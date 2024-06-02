using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

public interface IInboundContext
{
    string AuthenticationManagedIdentity(string resource);
    void SetHeader(string name, string value);
    void Base();
    void AuthenticationBasic(string username, string password);

    IContext Context { get; }
}