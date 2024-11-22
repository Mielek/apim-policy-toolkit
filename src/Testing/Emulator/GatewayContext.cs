// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

public class GatewayContext
{
    private readonly SectionContextProxy _inboundProxy;
    private readonly SectionContextProxy _outboundProxy;
    private readonly SectionContextProxy _backendProxy;
    private readonly SectionContextProxy _onErrorProxy;

    public MockExpressionContext RuntimeContext { get; } = new();

    // ReSharper disable SuspiciousTypeConversion.Global
    public IInboundContext InboundContext => (IInboundContext)_inboundProxy;
    public IOutboundContext OutboundContext => (IOutboundContext)_outboundProxy;
    public IBackendContext BackendContext => (IBackendContext)_backendProxy;
    public IOnErrorContext OnErrorContext => (IOnErrorContext)_onErrorProxy;
    // ReSharper restore SuspiciousTypeConversion.Global

    public GatewayContext()
    {
        _inboundProxy = SectionContextProxy.Create<IInboundContext>(this);
        _outboundProxy = SectionContextProxy.Create<IOutboundContext>(this);
        _backendProxy = SectionContextProxy.Create<IBackendContext>(this);
        _onErrorProxy = SectionContextProxy.Create<IOnErrorContext>(this);
    }

    public void SetHandler<T>(IInvokeHandler handler)
    {
        var scope = typeof(T).Name;
        var scopes = handler.GetType().GetCustomAttributes<SectionAttribute>().Select(att => att.Scope).ToArray();

        if (!scopes.Contains(scope))
        {
            throw new ArgumentException(
                $"Handler define {string.Join(',', scopes)} but is tried to be assigned to {scope} scope",
                nameof(handler));
        }

        var proxy = scope switch
        {
            nameof(IInboundContext) => _inboundProxy,
            nameof(IOutboundContext) => _outboundProxy,
            nameof(IBackendContext) => _backendProxy,
            nameof(IOnErrorContext) => _onErrorProxy,
            _ => throw new ArgumentException("Invalid policy type", nameof(T))
        };
        proxy.SetHandler(handler);
    }
}