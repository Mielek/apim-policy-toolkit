
using System.Text.Json.Nodes;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using Mielek.Generators.Common;

namespace Mielek.Generators.Model;

public class JsonToModelCompiler
{
    public readonly static DiagnosticDescriptor anyError = new DiagnosticDescriptor(
            "ANY",
            "Cannot get file text",
            "'{0}'",
            "JsonToCSharpCompiler",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
    private readonly GeneratorExecutionContext context;

    public JsonToModelCompiler(GeneratorExecutionContext context)
    {
        this.context = context;
    }

    public string Compile(string fileName, SourceText text)
    {
        try
        {
            var root = JsonNode.Parse(text.ToString())?.AsObject() ?? throw new NullReferenceException("root is null");
            return new InnerCompiler(context, fileName.ToPolicyClassName(), root, true).Compile();
        }
        catch (Exception e)
        {
            context.ReportDiagnostic(Diagnostic.Create(anyError, null, e.Message));
            return "";
        }
    }

    class InnerCompiler
    {
        readonly GeneratorExecutionContext context;
        readonly ModelClassBuilder builder;
        readonly JsonObject root;
        readonly string rootName;

        public InnerCompiler(GeneratorExecutionContext context, string name, JsonObject root, bool isPolicy)
        {
            this.context = context;
            rootName = name;
            builder = new ModelClassBuilder(name).WithAddPolicyInterfaces(isPolicy);

            this.root = root;
        }

        public string Compile()
        {
            try
            {
                foreach (var element in root)
                {
                    var propertyName = element.Key.ToCamelCase();
                    var node = element.Value?.AsObject() ?? throw new NullReferenceException(element.Key);
                    var description = CreateDescription(node);
                    switch (description.type)
                    {
                        case "object":
                            description = ProcessObject(description, node);
                            break;
                        case "array":
                            description = ProcessArray(description, node);
                            break;
                        case "policy":
                            description = description.WithType($"IPolicy");
                            break;
                    }
                    AddProperty(propertyName, description);
                }
            }
            catch (Exception e)
            {
                context.ReportDiagnostic(Diagnostic.Create(anyError, null, e.Message));
            }
            return builder.Build();
        }

        private PropertyDescription ProcessObject(PropertyDescription description, JsonObject node)
        {
            var newName = node.Name() ?? throw new NullReferenceException(node.GetPathTo("name"));
            description = description.WithType(rootName + newName.ToCamelCase());
            var newRoot = node.Properties();
            builder.WithSubClass(new InnerCompiler(context, description.type, newRoot, false).Compile());
            return description;
        }

        private PropertyDescription ProcessArray(PropertyDescription description, JsonObject node)
        {
            var itemsNode = node.Items();
            var itemsDescription = CreateDescription(itemsNode);
            if (itemsDescription.type == "object")
            {
                itemsDescription = ProcessObject(itemsDescription, itemsNode);
            }
            else if (itemsDescription.type == "policy")
            {
                itemsDescription = itemsDescription.WithType($"IPolicy");
            }

            if (itemsDescription.expression)
            {
                itemsDescription = itemsDescription.WithType($"IExpression<{itemsDescription.type}>");
            }

            description = description.WithType($"{itemsDescription.type}[]");
            return description;
        }

        private void AddProperty(string name, PropertyDescription description)
        {
            var enumValues = description.enumValues;
            if (enumValues != null)
            {
                builder.WithSubClass($"public enum {rootName}{name} {{ {string.Join(", ", enumValues.Select(v => v.Capitalize()))} }}");
            }

            if (description.expression)
            {
                description = description.WithType($"IExpression<{description.type}>");
            }

            if (description.optional)
            {
                description = description.WithType($"{description.type}?");
            }

            builder.WithProperty(name, description.type);
        }

        private PropertyDescription CreateDescription(JsonObject obj)
        {
            var type = obj.Type();
            var expression = obj.Expression();
            var optional = obj.Optional();
            var enumValues = obj.Enum();
            return new PropertyDescription(type, expression, optional, enumValues);
        }

        public record PropertyDescription(string type, bool expression, bool optional, string[]? enumValues)
        {
            public PropertyDescription WithType(string type) => new(type, expression, optional, enumValues);
        };

    }

}