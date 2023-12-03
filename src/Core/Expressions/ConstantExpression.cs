
using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

public sealed record ConstantExpression<T>(T Value) : IExpression<T>
{
    public string Source => $"{Value}";

    public XText GetXText() => new XText(Source);

    public XAttribute GetXAttribute(XName name) => new XAttribute(name, Source);

}