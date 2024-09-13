namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public record CheckHeaderConfig
{
    public required string Name { get; init; }
    public required string FailCheckHttpCode { get; init; }
    public required string FailCheckErrorMessage { get; init; }
    public required string IgnoreCase { get; init; }
    public required string[] Values { get; init; }
}