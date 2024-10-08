namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IOnErrorContext : IHaveExpressionContext
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void SetVariable(string name, dynamic value);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="method"></param>
    void SetMethod(string method);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="config"></param>
    void MockResponse(MockResponseConfig? config = null);

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
    /// Inlines the specified policy as is to policy document.
    /// </summary>
    /// <param name="policy">
    /// Policy in xml format.
    /// </param>
    void InlinePolicy(string policy);
}