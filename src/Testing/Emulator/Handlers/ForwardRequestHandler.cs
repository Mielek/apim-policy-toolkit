// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Handlers;

[Section(nameof(IBackendContext))]
public class ForwardRequestHandler: IInvokeHandler
{
    public Action<GatewayContext, ForwardRequestConfig?>? Interceptor { private get; init; }
    public string MethodName => nameof(IBackendContext.ForwardRequest);
    public object? Invoke(GatewayContext context, object?[]? args)
    {
        if (args == null || args.Length != 1)
        {
            throw new InvalidOperationException("ForwardRequest requires exactly one argument.");
        }

        ForwardRequestConfig? config = null;
        if (args[0] is not null)
        {
            if(args[0] is not ForwardRequestConfig c)
            {
                throw new InvalidOperationException("ForwardRequest requires a ForwardRequestConfig argument.");
            }
            config = c;
        }
        Interceptor?.Invoke(context, config);

        return null;
    }
}