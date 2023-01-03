using System.Xml;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class InlineScriptExpressionHandler<T> : MarshallerHandler<InlineScriptExpression<T>>
{
    public override void Marshal(Marshaller marshaller, InlineScriptExpression<T> element)
    {
        marshaller.Writer.WriteString($"@({element.Script})");
    }
}