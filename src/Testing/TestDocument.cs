// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

namespace Azure.ApiManagement.PolicyToolkit.Testing;

public class TestDocument(IDocument document)
{
    public GatewayContext Context { get; init; } = new();

    public void RunInbound() => document.Inbound(Context.InboundProxy.Object);
    public void RunBackend() => document.Backend(Context.BackendProxy.Object);
    public void RunOutbound() => document.Outbound(Context.OutboundProxy.Object);
    public void RunOnError() => document.OnError(Context.OnErrorProxy.Object);
}