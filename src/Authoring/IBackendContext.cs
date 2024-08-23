namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IBackendContext : IHaveExpressionContext
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void ForwardRequest(ForwardRequestConfig? config = null);

    /// <summary>
    /// Inlines the specified policy as is to policy document.
    /// </summary>
    /// <param name="policy">
    /// Policy in xml format.
    /// </param>
    void InlinePolicy(string policy);
}