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
public class SetVariableHandler : IPolicyHandler
{
    public List<Tuple<
        Func<GatewayContext, string, object, bool>,
        Action<GatewayContext, string, object>
    >> CallbackHooks { get; } = new();

    public string PolicyName => nameof(IInboundContext.SetVariable);

    public object? Handle(GatewayContext context, object?[]? args)
    {
        (string name, object value) = args.ExtractArguments<string, object>();

        var callbackHook = CallbackHooks.Find(hook => hook.Item1(context, name, value));
        if (callbackHook is not null)
        {
            callbackHook.Item2(context, name, value);
        }
        else
        {
            context.Variables[name] = value;
        }

        return null;
    }
}