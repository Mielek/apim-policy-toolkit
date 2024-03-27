using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

public interface IOutboundContext
{
    void RemoveHeader(string name);
    void SetBody(string body);
    void Base();
    
    IContext Context { get; }
}