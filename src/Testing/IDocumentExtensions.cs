// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing;

public static class IDocumentExtensions
{
    public static TestDocument AsTestDocument(this IDocument document) => new(document);
}