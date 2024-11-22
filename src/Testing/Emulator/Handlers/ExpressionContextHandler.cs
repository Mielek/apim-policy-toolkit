// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Handlers;

[
    Section(nameof(IInboundContext)),
    Section(nameof(IBackendContext)),
    Section(nameof(IOutboundContext)),
    Section(nameof(IOnErrorContext))
]
public class ExpressionContextHandler : IInvokeHandler
{
    public string MethodName => "get_ExpressionContext";
    public object? Invoke(GatewayContext context, object?[]? args) => context.RuntimeContext;
}