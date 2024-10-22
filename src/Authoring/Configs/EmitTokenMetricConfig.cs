namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public record EmitTokenMetricConfig
{
    public required MetricDimensionConfig[] Dimensions { get; init; }
    public string? Namespace { get; init; }
}