namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public record BasicAuthCredentials
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}