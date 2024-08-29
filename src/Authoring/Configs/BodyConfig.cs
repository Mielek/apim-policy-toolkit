namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class BodyConfig
{
    public required object? Content { get; init; }
    public string? Template { get; init; }
    public string? XsiNil { get; init; }
    public bool? ParseDate { get; init; }
}