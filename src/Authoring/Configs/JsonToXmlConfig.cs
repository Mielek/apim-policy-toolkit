// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record JsonToXmlConfig
{
    public required string Apply { get; init; }
    public bool? ConsiderAcceptHeader { get; init; }
    public bool? ParseDate { get; init; }
    public char? NamespaceSeparator { get; init; }
    public string? NamespacePrefix { get; init; }
    public string? AttributeBlockName { get; init; }
}