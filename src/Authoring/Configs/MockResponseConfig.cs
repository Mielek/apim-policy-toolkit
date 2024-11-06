// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record MockResponseConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    public int? StatusCode { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public string? ContentType { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public int? Index { get; init; }

}