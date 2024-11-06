// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public class MetricDimensionConfig
{
    public required string Name { get; init; }
    public string? Value { get; init; }
}