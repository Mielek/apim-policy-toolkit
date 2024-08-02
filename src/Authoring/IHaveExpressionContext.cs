using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IHaveExpressionContext
{
    IExpressionContext ExpressionContext { get; }
}