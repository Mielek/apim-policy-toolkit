
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Mielek.Generators.Model;

public class JsonToCSharpCompiler
{
    public readonly static DiagnosticDescriptor anyError = new DiagnosticDescriptor(
            "ANY",
            "Cannot get file text",
            "'{0}'",
            "JsonToCSharpCompiler",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
    private readonly GeneratorExecutionContext context;

    public JsonToCSharpCompiler(GeneratorExecutionContext context)
    {
        this.context = context;
    }

    public string Compile(string fileName, SourceText text)
    {
        try
        {
            var root = JsonNode.Parse(text.ToString())?.AsObject() ?? throw new NullReferenceException("root is null");
            return new InnerCompiler(fileName.ToCamelCase(), root, true).Compile();
        }
        catch (Exception e)
        {
            context.ReportDiagnostic(Diagnostic.Create(anyError, null, e.Message));
            return "";
        }
    }

    class InnerCompiler
    {
        readonly ModelClassBuilder builder;
        readonly JsonObject root;

        public InnerCompiler(string name, JsonObject root, bool isPolicy)
        {
            builder = new ModelClassBuilder(name).WithAddPolicyInterfaces(isPolicy);

            this.root = root;
        }

        public string Compile()
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
            return builder.Build();
        }

        private PropertyDescription ProcessObject(PropertyDescription description, JsonObject node)
        {
            var newName = node["name"]?.GetValue<string>() ?? throw new NullReferenceException("name");
            description = description.WithType(newName.ToCamelCase());
            var newRoot = node["properties"]?.AsObject() ?? throw new NullReferenceException("properties");
            builder.WithSubClass(new InnerCompiler(description.type, newRoot, false).Compile());
            return description;
        }

        private PropertyDescription ProcessArray(PropertyDescription description, JsonObject node)
        {
            var itemsNode = node["items"]?.AsObject() ?? throw new NullReferenceException("items");
            var itemsDescription = CreateDescription(itemsNode);
            if (itemsDescription.type == "object")
            {
                itemsDescription = ProcessObject(itemsDescription, itemsNode);
            }
            else if (itemsDescription.type == "policy")
            {
                itemsDescription = itemsDescription.WithType($"IPolicy");
            }

            description = description.WithType($"{itemsDescription.type}[]");
            return description;
        }

        private void AddProperty(string name, PropertyDescription description)
        {
            var enumValues = description.enumValues;
            if (enumValues != null)
            {
                builder.WithSubClass($"public enum {name}Enum {{ {string.Join(", ", enumValues.Select(v => v.Capitalize()))} }}");
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
            var type = obj["type"]?.AsValue().GetValue<string>() ?? throw new NullReferenceException("type");
            var expression = obj["expression"]?.AsValue().GetValue<bool>() ?? false;
            var optional = obj["optional"]?.AsValue().GetValue<bool>() ?? false;
            var enumValues = obj["enum"]?.AsArray().Select(v => v?.GetValue<string>() ?? throw new NullReferenceException("enum value")).ToArray();
            return new PropertyDescription(type, expression, optional, enumValues);
        }

        public record PropertyDescription(string type, bool expression, bool optional, string[]? enumValues)
        {
            public PropertyDescription WithType(string type) => new(type, expression, optional, enumValues);
        };

    }

}