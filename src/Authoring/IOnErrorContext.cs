namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IOnErrorContext : IHaveExpressionContext
{

    /// <summary>
    /// Inlines the specified policy as is to policy document.
    /// </summary>
    /// <param name="policy">
    /// Policy in xml format.
    /// </param>
    void InlinePolicy(string policy);
}