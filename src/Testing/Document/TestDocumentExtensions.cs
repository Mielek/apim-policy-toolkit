// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class TestDocumentExtensions
{
    public static MockPoliciesProvider<IInboundContext> MockInbound(this TestDocument document) =>
        new(document.Context.InboundProxy);

    public static MockPoliciesProvider<IBackendContext> MockBackend(this TestDocument document) =>
        new(document.Context.BackendProxy);

    public static MockPoliciesProvider<IOutboundContext> MockOutbound(this TestDocument document) =>
        new(document.Context.OutboundProxy);

    public static MockPoliciesProvider<IOnErrorContext> MockOnError(this TestDocument document) =>
        new(document.Context.OnErrorProxy);
}