// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

public class GatewayContext : MockExpressionContext
{
    private readonly SectionContextProxy _inboundProxy;
    private readonly SectionContextProxy _outboundProxy;
    private readonly SectionContextProxy _backendProxy;
    private readonly SectionContextProxy _onErrorProxy;

    public GatewayContext()
    {
        _inboundProxy = SectionContextProxy.Create<IInboundContext>(this);
        _outboundProxy = SectionContextProxy.Create<IOutboundContext>(this);
        _backendProxy = SectionContextProxy.Create<IBackendContext>(this);
        _onErrorProxy = SectionContextProxy.Create<IOnErrorContext>(this);
    }

    internal void SetHandler<T>(IPolicyHandler handler) where T : class
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
            nameof(IBackendContext) => _backendProxy,
            nameof(IOutboundContext) => _outboundProxy,
            nameof(IOnErrorContext) => _onErrorProxy,
            _ => throw new ArgumentException("Invalid policy type", nameof(T))
        };
        proxy.SetHandler(handler);
    }

    internal THandler GetHeader<TSection, THandler>() 
        where TSection : class
        where THandler : class, IPolicyHandler
    {
        var scope = typeof(TSection).Name;
        var proxy = scope switch
        {
            nameof(IInboundContext) => _inboundProxy,
            nameof(IBackendContext) => _backendProxy,
            nameof(IOutboundContext) => _outboundProxy,
            nameof(IOnErrorContext) => _onErrorProxy,
            _ => throw new ArgumentException("Invalid policy section", nameof(TSection))
        };
        return proxy.GetHandler<THandler>();
    }
    
    internal SectionContextProxy GetSectionProxy<TContext>() where TContext : class
    {
        var scope = typeof(TContext).Name;
        var context = scope switch
        {
            nameof(IInboundContext) => _inboundProxy,
            nameof(IBackendContext) => _backendProxy,
            nameof(IOutboundContext) => _outboundProxy,
            nameof(IOnErrorContext) => _onErrorProxy,
            _ => throw new ArgumentException("Invalid policy type", nameof(TContext))
        };
        return context;
    }

    internal TContext GetSectionContext<TContext>() where TContext : class
    {
        var context = GetSectionProxy<TContext>();
        return context as TContext ?? throw new InvalidOperationException("Invalid policy type");
    }
}