// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Handlers;

[Section(nameof(IInboundContext))]
public class SetHeaderRequestHandler : SetHeaderHandler
{
    protected override Dictionary<string, string[]> GetHeaders(GatewayContext context)
        => context.RuntimeContext.Request.Headers;
}

[Section(nameof(IOutboundContext)), Section(nameof(IOnErrorContext))]
public class SetHeaderResponseHandler : SetHeaderHandler
{
    protected override Dictionary<string, string[]> GetHeaders(GatewayContext context)
        => context.RuntimeContext.Response.Headers;
}

public abstract class SetHeaderHandler : IInvokeHandler
{
    public Action<MockExpressionContext, string, string[]>? Interceptor { private get; init; }
    public string MethodName => nameof(IInboundContext.SetHeader);

    public object? Invoke(GatewayContext context, object?[]? args)
    {
        if (args?.Length != 2)
        {
            throw new ArgumentException("SetHeader policy expects exactly 2 arguments.");
        }

        if (args[0] is not string name)
        {
            throw new ArgumentException("SetHeader policy expects the first argument to be a string.");
        }

        if (args[1] is not string[] values)
        {
            throw new ArgumentException("SetHeader policy expects the second argument to be an array of strings.");
        }

        if (Interceptor is not null)
        {
            Interceptor(context.RuntimeContext, name, values);
        }
        else
        {
            GetHeaders(context)[name] = values;
        }

        return null;
    }

    protected abstract Dictionary<string, string[]> GetHeaders(GatewayContext context);
}