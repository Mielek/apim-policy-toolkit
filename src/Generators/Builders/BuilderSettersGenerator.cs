using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder;

[Generator]
public class BuilderSettersGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        {
            var classBuilders = GenerateSetterBuilder(context.Compilation, syntaxTree);
            foreach (var classBuilder in classBuilders)
            {
                context.AddSource($"{classBuilder.Key}.Builder.cs", SourceText.From(classBuilder.Value, Encoding.UTF8));
            }
        }
    }

    private Dictionary<string, string> GenerateSetterBuilder(Compilation compilation, SyntaxTree syntaxTree)
    {
        var classToBuilder = new Dictionary<string, string>();

        var root = syntaxTree.GetRoot();
        var classesWithAttribute = root
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(cds => cds.AttributeLists.HasAttribute(nameof(GenerateBuilderSettersAttribute)))
            .ToList();

        foreach (var classDeclaration in classesWithAttribute)
        {
            var className = classDeclaration.Identifier.Text;
            classToBuilder[className] = new ClassSetterBuilder(classDeclaration).Build();
        }

        return classToBuilder;
    }

    public void Initialize(GeneratorInitializationContext context)
    {
            Console.Out.WriteLine("Test");
    }
}