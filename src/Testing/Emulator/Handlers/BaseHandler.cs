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
public class BaseHandler : IInvokeHandler
{
    public Action<GatewayContext>? Interceptor { private get; init; }
    public string MethodName => nameof(IInboundContext.Base);

    public object? Invoke(GatewayContext context, object?[]? args)
    {
        Interceptor?.Invoke(context);
        return null;
    }
}