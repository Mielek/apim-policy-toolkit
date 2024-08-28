using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IInboundContext : IHaveExpressionContext
{
    /// <summary>
    /// Adds header of specified name with values or overrides values if header already exists.<br />
    /// Compiled to <a href="https://learn.microsoft.com/en-us/azure/api-management/set-header-policy">set-header</a> policy.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be added. Policy expressions are allowed.
    /// </param>
    /// <param name="values">
    /// Specifies the values of the header to be set. Policy expressions are allowed.
    /// </param>
    void SetHeader(string name, params string[] values);

    /// <summary>
    /// Sets header of specified name and values if header not already exist.<br />
    /// Compiled to <a href="https://learn.microsoft.com/en-us/azure/api-management/set-header-policy">set-header</a> policy.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be added. Policy expressions are allowed.
    /// </param>
    /// <param name="values">
    /// Specifies the values of the header to be set. Policy expressions are allowed.
    /// </param>
    void SetHeaderIfNotExist(string name, params string[] values);

    /// <summary>
    /// Adds header of specified name with values or appends values if header already exists.<br />
    /// Compiled to <a href="https://learn.microsoft.com/en-us/azure/api-management/set-header-policy">set-header</a> policy.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be added. Policy expressions are allowed.
    /// </param>
    /// <param name="values">
    /// Specifies the values of the header to be set or appended. Policy expressions are allowed.
    /// </param>
    void AppendHeader(string name, params string[] values);

    /// <summary>
    /// Deletes header of specified name.<br />
    /// Compiled to <a href="https://learn.microsoft.com/en-us/azure/api-management/set-header-policy">set-header</a> policy.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be deleted. Policy expressions are allowed.
    /// </param>
    void RemoveHeader(string name);

    /// <summary>
    /// The base policy used to specify when parent scope policy should be executed
    /// </summary>
    void Base();

    /// <summary>
    /// The cors policy adds cross-origin resource sharing (CORS) support to an operation or an API to allow cross-domain calls from browser-based clients.
    /// </summary>
    /// <param name="config"></param>
    void Cors(CorsConfig config);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="body"></param>
    /// <param name="config"></param>
    void SetBody(string body, SetBodyConfig? config = null);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void SetVariable(string name, dynamic value);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void RateLimit(RateLimitConfig config);
    
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="template"></param>
    /// <param name="copyUnmatchedParams"></param>
    void RewriteUri(string template, bool copyUnmatchedParams = true);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void Quota(QuotaConfig config);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="method"></param>
    void SetMethod(string method);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void IpFilter(IpFilterConfig config);

    string AuthenticationManagedIdentity(string resource);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    void AuthenticationBasic(string username, string password);

    void Cache();
    void CacheLookup();
    void CacheStore();
    void SetBackendService();
    void Jsonp();
    void JsonToXml();
    void ValidateJwt();
    void ReturnResponse();
    void SendRequest();
    void RateLimitByKey();
    void SetQueryParameter();

    /// <summary>
    /// Inlines the specified policy as is to policy document.
    /// </summary>
    /// <param name="policy">
    /// Policy in xml format.
    /// </param>
    void InlinePolicy(string policy);
}