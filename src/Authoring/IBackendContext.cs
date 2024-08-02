namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IBackendContext : IHaveExpressionContext
{
    void ForwardRequest();

}