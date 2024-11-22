// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Handlers;

[Section(nameof(IInboundContext))]
public class SetBodyRequestHandler : SetBodyHandler
{
    protected override MockBody GetBody(GatewayContext context)
        => context.RuntimeContext.Request.Body;
}

[Section(nameof(IOutboundContext)), Section(nameof(IOnErrorContext))]
public class SetBodyResponseHandler : SetBodyHandler
{
    protected override MockBody GetBody(GatewayContext context)
        => context.RuntimeContext.Response.Body;
}

public abstract class SetBodyHandler : IInvokeHandler
{
    public Action<GatewayContext, string, SetBodyConfig?>? Interceptor { private get; init; }
    public string MethodName => nameof(IInboundContext.SetBody);

    public object? Invoke(GatewayContext context, object?[]? args)
    {
        if (args == null || args.Length != 2)
        {
            throw new InvalidOperationException();
        }

        if (args[0] is not string body)
        {
            throw new InvalidOperationException("SetBodyHandler requires a string argument.");
        }

        SetBodyConfig? config = null;

        if (args[1] is not null)
        {
            if (args[1] is not SetBodyConfig c)
            {
                throw new InvalidOperationException("SetBodyHandler requires a string argument for the content type.");
            }

            config = c;
        }

        if (Interceptor is not null)
        {
            Interceptor(context, body, config);
            return null;
        }

        var contextBody = GetBody(context);
        contextBody.Content = body;

        return null;
    }

    protected abstract MockBody GetBody(GatewayContext context);
}