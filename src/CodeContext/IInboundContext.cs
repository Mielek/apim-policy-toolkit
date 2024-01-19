using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

public interface IInboundContext
{
    string AuthenticationManagedIdentity(string resource);
    void SetHeader(string name, string value);
    void Base();
    void CheckHeader(string name, int errorCode, string errorMessage, bool igonoreCase, params string[] values);
    bool ContainsHeader(string name);
    void AuthenticationBasic(string username, string password);
    
    IContext Context { get; }
}