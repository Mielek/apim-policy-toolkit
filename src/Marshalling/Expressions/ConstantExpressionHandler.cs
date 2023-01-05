using System.Xml;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class ConstantExpressionHandler<T> : MarshallerHandler<ConstantExpression<T>>
{
    public override void Marshal(Marshaller marshaller, ConstantExpression<T> element)
    {
        marshaller.Writer.WriteRawString($"{element.Value}");
    }
}