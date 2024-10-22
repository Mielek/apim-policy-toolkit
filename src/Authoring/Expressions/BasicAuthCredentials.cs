namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public interface BasicAuthCredentials
{
    public string Username { get; }
    public string Password { get; }
}