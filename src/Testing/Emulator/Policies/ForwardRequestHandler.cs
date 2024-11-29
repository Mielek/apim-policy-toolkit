// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IBackendContext))]
internal class ForwardRequestHandler: IPolicyHandler
{
    public List<Tuple<
        Func<GatewayContext, ForwardRequestConfig?, bool>,
        Action<GatewayContext, ForwardRequestConfig?>
    >> CallbackHooks { get; } = new();

    public string PolicyName => nameof(IBackendContext.ForwardRequest);
    public object? Handle(GatewayContext context, object?[]? args)
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
        var callbackHook = CallbackHooks.Find(hook => hook.Item1(context, config));
        callbackHook?.Item2(context, config);

        // TODO
        
        return null;
    }
}