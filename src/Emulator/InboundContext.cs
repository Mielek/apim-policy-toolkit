using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Emulator;

public class InboundContext : IInboundContext
{
    public string AuthenticationManagedIdentity(string resource)
    {
        throw new NotImplementedException();
    }

    public void SetHeader(string name, params string[] values)
    {
        throw new NotImplementedException();
    }

    public void AddHeader(string name, params string[] values)
    {
        throw new NotImplementedException();
    }

    public void RemoveHeader(string name)
    {
        throw new NotImplementedException();
    }

    public void Base()
    {
        throw new NotImplementedException();
    }

    public void AuthenticationBasic(string username, string password)
    {
        throw new NotImplementedException();
    }

    public void ForwardRequest()
    {
        throw new NotImplementedException();
    }

    public void RateLimit()
    {
        throw new NotImplementedException();
    }

    public void Quota()
    {
        throw new NotImplementedException();
    }

    public void RewriteUri()
    {
        throw new NotImplementedException();
    }

    public void Cache()
    {
        throw new NotImplementedException();
    }

    public void CacheLookup()
    {
        throw new NotImplementedException();
    }

    public void CacheStore()
    {
        throw new NotImplementedException();
    }

    public void SetBackendService()
    {
        throw new NotImplementedException();
    }

    public void Jsonp()
    {
        throw new NotImplementedException();
    }

    public void JsonToXml()
    {
        throw new NotImplementedException();
    }

    public void Cors()
    {
        throw new NotImplementedException();
    }

    public void ValidateJwt()
    {
        throw new NotImplementedException();
    }

    public void SetVariable()
    {
        throw new NotImplementedException();
    }

    public void ReturnResponse()
    {
        throw new NotImplementedException();
    }

    public void SetBody()
    {
        throw new NotImplementedException();
    }

    public void SendRequest()
    {
        throw new NotImplementedException();
    }

    public void RateLimitByKey()
    {
        throw new NotImplementedException();
    }

    public void SetMethod()
    {
        throw new NotImplementedException();
    }

    public void SetQueryParameter()
    {
        throw new NotImplementedException();
    }

    public IContext Context { get; }
}