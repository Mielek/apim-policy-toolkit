using System.Xml.Linq;

namespace Mielek.Builders.Expressions;

public sealed record InlineExpression<T>(string Expression) : IExpression<T>
{
    public string Source => $"@{Expression}";

    public XText GetXText() => new XText(Source);

    public XAttribute GetXAttribute(XName name) => new XAttribute(name, Source);

}