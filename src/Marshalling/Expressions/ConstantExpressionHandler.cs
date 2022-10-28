using System.Xml;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class ConstantExpressionHandler : MarshallerHandler<ConstantExpression>
{
    public override void Marshal(Marshaller marshaller, ConstantExpression element)
    {
        marshaller.Writer.WriteString(element.Value);
    }
}