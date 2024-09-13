namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public record SetBodyConfig
{
    public string? Template { get; init; }
    public string? XsiNil { get; init; }
    public bool? ParseDate { get; init; }
}