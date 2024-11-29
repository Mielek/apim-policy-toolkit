// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class AuthenticationManagedIdentityHandler : IPolicyHandler
{
    public List<Tuple<
        Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool>,
        Action<GatewayContext, ManagedIdentityAuthenticationConfig>
        >> CallbackHooks { get; } = new();

    public List<Tuple<
            Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool>,
            Func<string, string?, string>
    >> ProvideTokenHooks { get; } = new();

    public string PolicyName => nameof(IInboundContext.AuthenticationManagedIdentity);

    public object? Handle(GatewayContext context, object?[]? args)
    {
        var config = args.ExtractArgument<ManagedIdentityAuthenticationConfig>();

        var callbackHook = CallbackHooks.Find(hook => hook.Item1(context, config));
        if (callbackHook is not null)
        {
            callbackHook.Item2(context, config);
            return null;
        }

        var provideTokenHook = ProvideTokenHooks.FirstOrDefault(hook => hook.Item1(context, config));

        var token = provideTokenHook is not null
            ? CreateTokenByHook(provideTokenHook.Item2, config)
            : DefaultTokenProvider(context, config);

        if (!string.IsNullOrWhiteSpace(config.OutputTokenVariableName))
        {
            context.Variables[config.OutputTokenVariableName] = token;
        }
        else
        {
            context.Request.Headers["Authorization"] = [$"Bearer {token}"];
        }

        return null;
    }

    private string DefaultTokenProvider(GatewayContext context, ManagedIdentityAuthenticationConfig config)
    {
        string token = $"resource={config.Resource}";
        if (!string.IsNullOrWhiteSpace(config.ClientId))
        {
            token += $"&client_id={config.ClientId}";
        }

        if (config.IgnoreError is not null)
        {
            token += $"&ignore_error={config.IgnoreError}";
        }

        return token;
    }

    private string CreateTokenByHook(Func<string, string?, string> tokenProvider,
        ManagedIdentityAuthenticationConfig config)
    {
        string token;
        try
        {
            token = tokenProvider(config.Resource, config.ClientId);
        }
        catch
        {
            if (config.IgnoreError ?? false)
            {
                token = "";
            }
            else
            {
                throw;
            }
        }

        return token;
    }
}