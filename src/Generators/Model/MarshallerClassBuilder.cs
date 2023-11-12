using System.Text;

namespace Mielek.Generators.Model;

public class MarshallerClassBuilder
{
    private Stack<string> closings = new Stack<string>();
    private StringBuilder builder = new StringBuilder();
    private int indent = 0;
    public MarshallerClassBuilder(string elementName, string className)
    {
        builder.AppendLine("using Mielek.Model.Policies;");
        builder.AppendLine("namespace Mielek.Marshalling.Policies;");
        builder.AppendLine($"public class {className}Handler : MarshallerHandler<{className}>");
        AppendBrackets();
        builder.AppendLine($"public override void Marshal(Marshaller marshaller, {className} element)".Intend(indent));
        AppendBrackets();
        AppendStartElement(elementName);
    }

    public MarshallerClassBuilder AppendAttribute(string attributeName, string accessor)
    {
        builder.AppendLine($"marshaller.Writer.WriteNullableAttribute(\"{attributeName}\", {accessor});".Intend(indent));
        return this;
    }

    public MarshallerClassBuilder AppendElement(string elementName, string accessor)
    {
        builder.AppendLine($"marshaller.Writer.WriteNullableElement(\"{elementName}\", {accessor});".Intend(indent));
        return this;
    }

    
    public MarshallerClassBuilder AppendStartElement(string elementName)
    {
        builder.AppendLine($"marshaller.Writer.WriteStartElement(\"{elementName}\");".Intend(indent));
        closings.Push("marshaller.Writer.WriteEndElement();".Intend(indent));
        return this;
    }

    public MarshallerClassBuilder AppendAcceptMarshaller(string varName)
    {
        builder.AppendLine($"{varName}.Accept(marshaller);".Intend(indent));
        return this;
    }

    public MarshallerClassBuilder AppendForEach(string varName, string iterable)
    {
        builder.AppendLine($"foreach (var {varName} in {iterable})".Intend(indent));
        return AppendBrackets();
    }

    public MarshallerClassBuilder AppendIfNotNull(string accessor)
    {
        builder.AppendLine($"if ({accessor} != null)".Intend(indent));
        return AppendBrackets();
    }

    public MarshallerClassBuilder AppendBrackets()
    {
        builder.AppendLine("{".Intend(indent));
        closings.Push("}".Intend(indent));
        indent++;
        return this;
    }

    public MarshallerClassBuilder FinishPart()
    {
        if(closings.TryPop(out var result))
        {
            builder.AppendLine(result);
            var newIndent = result.TakeWhile(c => c == ' ').Count() / 4;
            if(newIndent < indent)
            {
                indent = newIndent;
            }
        }
        return this;
    }

    public string Build()
    {
        while (closings.TryPop(out var result))
        {
            builder.AppendLine(result);
        }
        return builder.ToString();
    }

}