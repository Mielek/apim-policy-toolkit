
using System.Text.Json.Nodes;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using Mielek.Generators.Common;

namespace Mielek.Generators.Marshaller;

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
            return new InnerCompiler(context, fileName, root).Compile();
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
        readonly MarshallerClassBuilder builder;
        readonly JsonObject root;

        public InnerCompiler(GeneratorExecutionContext context, string name, JsonObject root)
        {
            this.context = context;
            builder = new MarshallerClassBuilder(name, name.ToPolicyClassName());
            this.root = root;
        }

        public string Compile()
        {
            try
            {
                foreach (var element in root)
                {
                    var node = element.Value?.AsObject() ?? throw new NullReferenceException(root.GetPathTo(element.Key));
                    ProcessDescription(element.Key, node);
                }

            }
            catch (Exception e)
            {
                context.ReportDiagnostic(Diagnostic.Create(anyError, null, e.Message));
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
                    if (target == "element")
                    {
                        builder.AppendStartElement(elementName);
                    }
                    builder.AppendForEach(propertyName.VariableName(), propertyName);
                    ProcessArray(value.Items());
                    builder.FinishPart(true);
                    if (target == "element")
                    {
                        builder.FinishPart();
                    }
                    break;
                case "object":
                    ProcessObject(propertyName, value);
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

        private void ProcessObject(string propertyName, JsonObject value)
        {
            var implicitName = value.Name();
            if (implicitName != null)
            {
                builder.AppendStartElement(implicitName);
            }
            builder.AppendVariable(propertyName.VariableName(), propertyName);

            foreach (var element in value.Properties())
            {
                var node = element.Value?.AsObject() ?? throw new NullReferenceException(value.GetPathTo(element.Key));
                ProcessDescription(element.Key, node);
            }

            if (implicitName != null)
            {
                builder.FinishPart(true);
            }
            else
            {
                builder.PopVariable();
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