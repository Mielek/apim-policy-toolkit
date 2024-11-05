namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record EmitMetricConfig
{
    
    public required string Name { get; init; }
    public required MetricDimensionConfig[] Dimensions { get; init; }
    
    public string? Namespace { get; init; }
    public double? Value { get; init; }
}