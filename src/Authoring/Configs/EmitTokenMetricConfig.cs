// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record EmitTokenMetricConfig
{
    public required MetricDimensionConfig[] Dimensions { get; init; }
    public string? Namespace { get; init; }
}