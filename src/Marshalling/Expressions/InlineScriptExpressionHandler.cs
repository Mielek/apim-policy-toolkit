using System.Xml;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class InlineScriptExpressionHandler : MarshallerHandler<InlineScriptExpression>
{
    public override void Marshal(Marshaller marshaller, InlineScriptExpression element)
    {
        marshaller.Writer.WriteString($"@({element.Script})");
    }
}