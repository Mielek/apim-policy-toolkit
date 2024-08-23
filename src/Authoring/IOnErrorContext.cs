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
    /// Inlines the specified policy as is to policy document.
    /// </summary>
    /// <param name="policy">
    /// Policy in xml format.
    /// </param>
    void InlinePolicy(string policy);
}