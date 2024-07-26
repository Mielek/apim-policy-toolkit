using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IOutboundContext
{
    void RemoveHeader(string name);
    void SetBody(string body);
    void Base();

    IContext Context { get; }
}