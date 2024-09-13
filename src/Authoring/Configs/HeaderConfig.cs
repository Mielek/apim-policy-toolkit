namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;


public record HeaderConfig
{
    public required string Name { get; init; }
    public string? ExistsAction { get; init; }
    public required string[] Values { get; init; }
}