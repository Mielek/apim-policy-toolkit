namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class ProxyConfig
{
    public required string Url { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
}