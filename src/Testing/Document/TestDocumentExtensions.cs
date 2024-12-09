// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class TestDocumentExtensions
{
    public static MockPoliciesProvider<IInboundContext> InInbound(this TestDocument document) =>
        new(document.Context.InboundProxy);

    public static MockPoliciesProvider<IBackendContext> InBackend(this TestDocument document) =>
        new(document.Context.BackendProxy);

    public static MockPoliciesProvider<IOutboundContext> InOutbound(this TestDocument document) =>
        new(document.Context.OutboundProxy);

    public static MockPoliciesProvider<IOnErrorContext> InOnError(this TestDocument document) =>
        new(document.Context.OnErrorProxy);
}