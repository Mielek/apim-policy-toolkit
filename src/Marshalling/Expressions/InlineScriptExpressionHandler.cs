using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class InlineScriptExpressionHandler<T> : MarshallerHandler<InlineScriptExpression<T>>
{
    public override void Marshal(Marshaller marshaller, InlineScriptExpression<T> element)
    {
        marshaller.Writer.WriteRawString($"@({element.Script})");
    }
}