using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Mielek.Generator.Attributes;

namespace BuilderGenerator
{
    //[Generator]
    public class BuilderSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // using the context, get a list of syntax trees in the users compilation
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var classBuilders = GenerateBuilder(context.Compilation, syntaxTree);
                // add the filepath of each tree to the class we're building
                foreach (var classBuilder in classBuilders)
                {
                    context.AddSource($"{classBuilder.Key}.Builder.cs", SourceText.From(classBuilder.Value, Encoding.UTF8));
                }

            }
            // inject the created source into the users compilation
        }

        public static Dictionary<string, string> GenerateBuilder(Compilation compilation, SyntaxTree syntaxTree)
        {
            var classToBuilder = new Dictionary<string, string>();

            var root = syntaxTree.GetRoot();
            var classesWithAttribute = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(cds => cds.AttributeLists.HasAttribute(nameof(GenerateBuilderAttribute)))
                .ToList();

            foreach (var classDeclaration in classesWithAttribute)
            {
                var att = classDeclaration.AttributeLists.SelectMany(a => a.Attributes).Where(a => a.Name.ToString().StartsWith("GenerateBuilder")).First();
                var arg = (TypeOfExpressionSyntax) att.ArgumentList?.Arguments[0].Expression;
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var symbol = (INamedTypeSymbol) semanticModel.GetSymbolInfo(arg.Type).Symbol;
                var parameters = symbol.Constructors[0].Parameters;

                var t = symbol.GetMembers().OfType<IPropertySymbol>().Where(p => p.DeclaredAccessibility == Accessibility.Public);
                var namespaceName = classDeclaration.FindParent<NamespaceDeclarationSyntax>().Name.ToString();
                var className = classDeclaration.Identifier.Text;
                var fields = classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
                classToBuilder[className] = $@"
namespace {namespaceName};

public partial class {className}
{{
    public string Fields => ""{string.Join(",", fields.Select(f => f.Declaration.Variables[0].ToString()).ToArray())}"";
    public string ConstructorParamNames => ""{string.Join(",", parameters.Select(p => p.Name))}"";
    public string ConstructorParamTypes => ""{string.Join(",", parameters.Select(p => p.Type))}"";
    public string Arg => ""{string.Join(",", t.Select(f => f.Name.Replace(symbol.Name, "") ))}"";
}}
";
            }

            return classToBuilder;
        }

        public static Dictionary<string, string> GenerateBuilder_OLD(SyntaxTree syntaxTree)
        {
            var builderTemplate = @"
using System;
using System.Collections.Generic;
namespace @namespace
{
    @usings
    partial class @className
    {
        @constructor
        public static @builderName Builder => new @builderName();
        public class @builderName
        {
            @builderMethods
            public @className Build()
            {
                Validate();
                return new @className
                {
                    @propertiesCopy
                };
            }
            public void Validate()
            {
                void AddError(Dictionary<string, string> items, string property, string message)
                {
                    if (items.TryGetValue(property, out var errors))
                        items[property] = $""{errors}\n{message}"";
                    else
                        items[property] = message;
                }
                Dictionary<string,string> errors = new Dictionary<string, string>();
                @validations
                if(errors.Count > 0)
                    throw new BuilderCommon.BuilderException(errors);
            }
        }
    }
}";
            var builderMethodsTemplate = @"
            private @propertyType @backingField;
            public @builderName @propertyName(@propertyType @propertyName)
            {
                @backingField = @propertyName;
                return this;
            }
            ";

            var classToBuilder = new Dictionary<string, string>();

            var root = syntaxTree.GetRoot();
            var usings = (root as CompilationUnitSyntax).Usings.ToString();
            var classesWithAttribute = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(cds => cds.AttributeLists.HasAttribute(nameof(GenerateBuilderAttribute)))
                .ToList();

            foreach (var classDeclaration in classesWithAttribute)
            {
                var sb = new StringBuilder();
                var namespaceName = classDeclaration.FindParent<NamespaceDeclarationSyntax>().Name.ToString();
                var className = classDeclaration.Identifier.Text;
                var properties = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();
                var builderName = $"{className}Builder";
                var hasDefaultConstructor = classDeclaration
                    .DescendantNodes()
                    .OfType<ConstructorDeclarationSyntax>()
                    .Any(x => x.ParameterList.Parameters.Count == 0);
                var defaultConstructorBody = hasDefaultConstructor ? string.Empty : $"private {className}(){{}}";
                var builderMethods = new StringBuilder();
                foreach (var property in properties.Where(x => x.Modifiers.All(m => m.ToString() != "static")).Select(x => new BuilderPropertyInfo(x)))
                {
                    builderMethods.AppendLine(builderMethodsTemplate
                        .Replace("@builderName", builderName)
                        .Replace("@backingField", property.BackingFieldName)
                        .Replace("@propertyName", property.Name)
                        .Replace("@propertyType", property.Type));
                }

                var propertiesCopy = new StringBuilder();

                foreach (var property in properties.Select(x => new BuilderPropertyInfo(x)))
                {

                    propertiesCopy.AppendLine($"{property.Name} = {property.BackingFieldName},");
                }


                var validations = new StringBuilder();
                var requiredProperties = properties.Where(p => p.AttributeLists.HasAttribute("Required"));
                foreach (var property in requiredProperties.Select(x => new BuilderPropertyInfo(x)))
                {
                    validations.AppendLine($@"if({property.BackingFieldName} == default)  AddError(errors, ""{property.Name}"", ""Value is required"");");
                }

                sb.AppendLine(builderTemplate
                    .Replace("@namespace", namespaceName)
                    .Replace("@className", className)
                    .Replace("@constructor", defaultConstructorBody)
                    .Replace("@builderName", builderName)
                    .Replace("@usings", usings)
                    .Replace("@validations", validations.ToString())
                    .Replace("@builderMethods", builderMethods.ToString())
                    .Replace("@propertiesCopy", propertiesCopy.ToString()));

                classToBuilder[className] = sb.ToString();
            }


            return classToBuilder;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        struct BuilderPropertyInfo
        {
            public BuilderPropertyInfo(PropertyDeclarationSyntax property) : this()
            {
                Type = property.Type.ToString();
                Name = property.Identifier.ToString();
                ParameterName = $"{Name[0].ToString().ToLower()}{Name.Remove(0, 1)}";
                BackingFieldName = $"_{ParameterName}";
            }

            public string Name { get; set; }
            public string Type { get; set; }
            public string ParameterName { get; set; }
            public string BackingFieldName { get; set; }
        }
    }

}