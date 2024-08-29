namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;


public class HeaderConfig
{
    public required string Name { get; init; }
    public string? ExistsAction { get; init; }
    public required string[] Values { get; init; }
}