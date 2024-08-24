using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder;

[Generator]
public class SectionBuilderGenerator : ISourceGenerator
{
    static readonly string AddToSectionBuilder = nameof(AddToSectionBuilderAttribute).Remove(nameof(AddToSectionBuilderAttribute).Length - 9, 9);

    public void Execute(GeneratorExecutionContext context)
    {
        var compilation = context.Compilation;
        foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        {
            var root = syntaxTree.GetRoot();
            var model = compilation.GetSemanticModel(syntaxTree);
            root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(cds => cds.AttributeLists.HasAttribute(AddToSectionBuilder))
                .ToList()
                .ForEach(classDeclaration =>
                {
                    var symbol = model.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol ?? throw new Exception("Cannot find symbol model");
                    var className = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                    var methodName = className.Replace("PolicyBuilder", "");
                    if (symbol.IsGenericType)
                    {
                        methodName = methodName.Remove(methodName.IndexOf('<'));
                        className = $"{className.Remove(className.IndexOf('<'))}<{{0}}>";
                    }
                    classDeclaration.AttributeLists
                        .SelectMany(l => l.Attributes)
                        .Where(a => a.IsOfName(AddToSectionBuilder))
                        .Select(a => a.ArgumentList)
                        .Select(list => list?.Arguments.FirstOrDefault()?.Expression)
                        .Where(expression => expression != null)
                        .OfType<TypeOfExpressionSyntax>()
                        .Select(s => s.Type)
                        .ToList()
                        .ForEach(sectionType =>
                        {
                            var sectionSymbol = model.GetSymbolInfo(sectionType).Symbol ?? throw new Exception("No declared symbol for section type");
                            var sectionTypeName = sectionSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                            var sectionNamespaceName = sectionSymbol.ContainingNamespace.ToDisplayString();
                            var builder = new BuilderClassBuilder(sectionNamespaceName, sectionTypeName);

                            if (!SymbolEqualityComparer.Default.Equals(sectionSymbol.ContainingNamespace, symbol.ContainingNamespace))
                            {
                                builder.Using(symbol.ContainingNamespace.ToDisplayString());
                            }
                            var actualClassName = string.Format(className, sectionTypeName);

                            builder.Method(new BuilderSetMethod(
                                methodName,
                                new[] { $"Action<{actualClassName}> configurator" },
                                new[] {
                                    $"var builder = new {actualClassName}();",
                                    $"configurator(builder);",
                                    $"_sectionPolicies.Add(builder.Build());"
                                }
                            ));

                            var code = builder.Build();
                            context.AddSource($"{sectionType.GetText()}.{methodName}.g.cs", code);
                        });
                });
        }
    }


    public void Initialize(GeneratorInitializationContext context)
    {
    }
}