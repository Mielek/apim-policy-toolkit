using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

public interface IExpression<T>
{
    string Source { get; }

    XText GetXText();

    XAttribute GetXAttribute(XName name);
}