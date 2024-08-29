namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class BasicAuthenticationConfig : IAuthenticationConfig
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}