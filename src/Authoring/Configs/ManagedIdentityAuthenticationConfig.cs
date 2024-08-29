namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class ManagedIdentityAuthenticationConfig : IAuthenticationConfig
{
    public required string Resource { get; init; }
    public string? ClientId { get; init; }
    public string? OutputTokenVariableName { get; init; }
    public bool? IgnoreError { get; init; }
}