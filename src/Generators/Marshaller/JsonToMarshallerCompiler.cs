
using System.Text.Json.Nodes;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Mielek.Generators.Model;

public class JsonToMarshallerCompiler
{
    public readonly static DiagnosticDescriptor anyError = new DiagnosticDescriptor(
            "ANY",
            "Cannot get file text",
            "'{0}'",
            "JsonToCSharpCompiler",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
    private readonly GeneratorExecutionContext context;

    public JsonToMarshallerCompiler(GeneratorExecutionContext context)
    {
        this.context = context;
    }

    public string Compile(string fileName, SourceText text)
    {
        try
        {
            var root = JsonNode.Parse(text.ToString())?.AsObject() ?? throw new NullReferenceException("root is null");
            return new InnerCompiler(fileName, root).Compile();
        }
        catch (Exception e)
        {
            context.ReportDiagnostic(Diagnostic.Create(anyError, null, e.Message));
            return "";
        }
    }

    class InnerCompiler
    {
        readonly MarshallerClassBuilder builder;
        readonly JsonObject root;

        public InnerCompiler(string name, JsonObject root)
        {
            builder = new MarshallerClassBuilder(name, name.ToCamelCase());

            this.root = root;
        }

        public string Compile()
        {
            foreach (var element in root)
            {
                var node = element.Value?.AsObject() ?? throw new NullReferenceException(root.GetPathTo(element.Key));
                ProcessDescription(element.Key, node);
            }
            return builder.Build();
        }

        private void ProcessDescription(string elementName, JsonObject value)
        {
            var propertyName = elementName.ToCamelCase();
            var type = value.Type();
            var target = value.Target();
            if (value.Optional() && (type == "array" || type == "object"))
            {
                builder.AppendIfNotNull(propertyName);
            }

            switch (type)
            {
                case "array":
                    if(target == "element")
                    {
                        builder.AppendStartElement(elementName);
                    }
                    builder.AppendForEach(propertyName.VariableName(), propertyName);
                    ProcessArray(value.Items());
                    builder.FinishPart();
                    if(target == "element")
                    {
                        builder.FinishPart();
                    }
                    break;
                case "object":
                    ProcessObject(value);
                    break;
                case "policy":
                    builder.AppendAcceptMarshaller();
                    break;
                default:
                    switch (target)
                    {
                        case "element":
                            builder.AppendElement(elementName);
                            break;
                        case "attribute":
                            builder.AppendAttribute(elementName, propertyName);
                            break;
                        default:
                            throw new Exception($"Wrong target type. '{target}' under {value.GetPathTo("target")}");
                    }
                    break;
            }

            if (value.Optional() && (type == "array" || type == "object"))
            {
                builder.FinishPart();
            }
        }

        private void ProcessObject(JsonObject value)
        {
            var implicitName = value.Name();
            if(implicitName != null)
            {
                builder.AppendStartElement(implicitName);
            }
            foreach (var element in value.Properties())
            {
                var node = element.Value?.AsObject() ?? throw new NullReferenceException(value.GetPathTo(element.Key));
                ProcessDescription(element.Key, node);
            }
            if(implicitName != null)
            {
                builder.FinishPart();
            }
        }

        private void ProcessArray(JsonObject value)
        {
            if (value.Type() == "policy")
            {
                builder.AppendAcceptMarshaller();
            }
            else
            {
                var name = value.Name() ?? throw new NullReferenceException(value.GetPathToName());
                ProcessDescription(name, value);
            }
        }

    }

}