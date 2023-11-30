using System.Xml.Linq;

namespace Mielek.Builders.Expressions;

public interface IExpression<T> {
    XText GetXText();
}