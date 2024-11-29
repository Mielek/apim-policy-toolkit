// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class AuthenticationBasicHandler : IPolicyHandler
{
    public List<Tuple<
        Func<GatewayContext, string, string, bool>,
        Action<GatewayContext, string, string>
    >> CallbackHooks { get; } = new();

    public string PolicyName => nameof(IInboundContext.AuthenticationBasic);

    public object? Handle(GatewayContext context, object?[]? args)
    {
        (string username, string password) = args.ExtractArguments<string, string>();

        var callbackHook = CallbackHooks.Find(hook => hook.Item1(context, username, password));
        if (callbackHook is not null)
        {
            callbackHook.Item2(context, username, password);
        }
        else
        {
            var authHeader = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"))}";
            context.Request.Headers["Authorization"] = [authHeader];
        }

        return null;
    }
}