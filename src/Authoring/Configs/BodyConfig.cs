namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public record BodyConfig : SetBodyConfig
{
    public required object? Content { get; init; }
}