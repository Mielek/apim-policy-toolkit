using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class InlineExpressionHandler<T> : MarshallerHandler<InlineExpression<T>>
{
    public override void Marshal(Marshaller marshaller, InlineExpression<T> element)
    {
        marshaller.Writer.WriteRawString($"@({element.Expression})");
    }
}
