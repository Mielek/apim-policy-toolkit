using System.Text;

using Mielek.Generators.Common;

namespace Mielek.Generators.Marshaller;

public class MarshallerClassBuilder
{
    private Stack<string> closingsStack = new Stack<string>();
    private Stack<string> varNameStack = new Stack<string>();
    private StringBuilder builder = new StringBuilder();
    private int indent = 0;
    public MarshallerClassBuilder(string elementName, string className)
    {
        builder.AppendLine($"public class {className}Handler : MarshallerHandler<{className}>");
        AppendBrackets();
        builder.AppendLine($"public override void Marshal(Marshaller marshaller, {className} element)".Intend(indent));
        varNameStack.Push("element");
        AppendBrackets();
        AppendStartElement(elementName);
    }

    string CurrentVarName => varNameStack.Peek();

    public MarshallerClassBuilder AppendAttribute(string attributeName, string accessor)
    {
        builder.AppendLine($"marshaller.Writer.WriteNullableAttribute(\"{attributeName}\", {CurrentVarName}.{accessor});".Intend(indent));
        return this;
    }

    public MarshallerClassBuilder AppendElement(string elementName)
    {
        builder.AppendLine($"marshaller.Writer.WriteNullableElement(\"{elementName}\", {CurrentVarName});".Intend(indent));
        return this;
    }

    public MarshallerClassBuilder AppendStartElement(string elementName)
    {
        builder.AppendLine($"marshaller.Writer.WriteStartElement(\"{elementName}\");".Intend(indent));
        closingsStack.Push("marshaller.Writer.WriteEndElement();".Intend(indent));
        return this;
    }

    public MarshallerClassBuilder AppendAcceptMarshaller()
    {
        builder.AppendLine($"{CurrentVarName}.Accept(marshaller);".Intend(indent));
        return this;
    }

    public MarshallerClassBuilder AppendForEach(string varName, string accessor)
    {
        builder.AppendLine($"foreach (var {varName} in {CurrentVarName}.{accessor})".Intend(indent));
        varNameStack.Push(varName);
        return AppendBrackets();
    }

    public MarshallerClassBuilder AppendIfNotNull(string accessor)
    {
        builder.AppendLine($"if ({CurrentVarName}.{accessor} != null)".Intend(indent));
        return AppendBrackets();
    }

    public MarshallerClassBuilder AppendBrackets()
    {
        builder.AppendLine($"{{ //{indent}".Intend(indent));
        closingsStack.Push("}".Intend(indent));
        indent += 1;
        return this;
    }

    public MarshallerClassBuilder AppendVariable(string varName, string accessor)
    {
        builder.AppendLine($"var {varName} = {CurrentVarName}.{accessor};".Intend(indent));
        varNameStack.Push(varName);
        return this;
    }

    public MarshallerClassBuilder PopVariable()
    {
        varNameStack.Pop();
        return this;
    }

    public MarshallerClassBuilder FinishPart(bool popVarStack = false)
    {
        if (closingsStack.TryPop(out var result))
        {
            builder.AppendLine(result);
            var newIndent = result.TakeWhile(c => c == ' ').Count() / 4;
            if (newIndent < indent)
            {
                indent = newIndent;
            }

            if (popVarStack)
            {
                varNameStack.Pop();
            }
        }
        return this;
    }

    public string Build()
    {
        while (closingsStack.TryPop(out var result))
        {
            builder.AppendLine(result);
        }
        return builder.ToString();
    }

}