// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class SetHeaderRequestHandler : SetHeaderHandler
{
    protected override Dictionary<string, string[]> GetHeaders(GatewayContext context)
        => context.Request.Headers;
}

[Section(nameof(IOutboundContext)), Section(nameof(IOnErrorContext))]
internal class SetHeaderResponseHandler : SetHeaderHandler
{
    protected override Dictionary<string, string[]> GetHeaders(GatewayContext context)
        => context.Response.Headers;
}

internal abstract class SetHeaderHandler : IPolicyHandler
{
    public List<Tuple<
        Func<GatewayContext, string, string[], bool>,
        Action<GatewayContext, string, string[]>
    >> CallbackHooks { get; } = new();

    public string PolicyName => nameof(IInboundContext.SetHeader);

    public object? Handle(GatewayContext context, object?[]? args)
    {
        (string name, string[] values) = args.ExtractArguments<string, string[]>();

        var callbackHook = CallbackHooks.Find(hook => hook.Item1(context, name, values));
        if (callbackHook is not null)
        {
            callbackHook.Item2(context, name, values);
        }
        else
        {
            GetHeaders(context)[name] = values;
        }

        return null;
    }

    protected abstract Dictionary<string, string[]> GetHeaders(GatewayContext context);
}