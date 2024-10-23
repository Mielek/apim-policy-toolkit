namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IBackendContext : IHaveExpressionContext
{
    /// <summary>
    /// The base policy used to specify when parent scope policy should be executed
    /// </summary>
    void Base();

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void ForwardRequest(ForwardRequestConfig? config = null);

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
    void SendRequest(SendRequestConfig config);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void ReturnResponse(ReturnResponseConfig config);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void SetBackendService(SetBackendServiceConfig config);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void EmitMetric(EmitMetricConfig config);

    /// <summary>
    /// Inlines the specified policy as is to policy document.
    /// </summary>
    /// <param name="policy">
    /// Policy in xml format.
    /// </param>
    void InlinePolicy(string policy);
}