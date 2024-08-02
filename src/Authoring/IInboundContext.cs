using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IInboundContext : IHaveExpressionContext
{
    void SetHeader(string name, params string[] values);
    void AppendHeader(string name, params string[] values);
    void RemoveHeader(string name);
    void Base();

    string AuthenticationManagedIdentity(string resource);
    void AuthenticationBasic(string username, string password);

    void RateLimit();
    void Quota();
    void RewriteUri();
    void Cache();
    void CacheLookup();
    void CacheStore();
    void SetBackendService();
    void Jsonp();
    void JsonToXml();
    void Cors();
    void ValidateJwt();
    void SetVariable();
    void ReturnResponse();
    void SetBody();
    void SendRequest();
    void RateLimitByKey();
    void SetMethod();
    void SetQueryParameter();
}