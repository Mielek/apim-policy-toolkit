namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class MetricDimensionConfig
{
    public required string Name { get; init; }
    public string? Value { get; init; }
}