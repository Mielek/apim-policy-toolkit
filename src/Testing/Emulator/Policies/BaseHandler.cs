// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[
    Section(nameof(IInboundContext)),
    Section(nameof(IBackendContext)),
    Section(nameof(IOutboundContext)),
    Section(nameof(IOnErrorContext))
]
public class BaseHandler : IPolicyHandler
{
    public Action<GatewayContext>? Interceptor { private get; init; }
    public string PolicyName => nameof(IInboundContext.Base);

    public object? Handle(GatewayContext context, object?[]? args)
    {
        Interceptor?.Invoke(context);
        return null;
    }
}