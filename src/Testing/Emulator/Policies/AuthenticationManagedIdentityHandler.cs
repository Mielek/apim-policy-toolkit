// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class AuthenticationManagedIdentityHandler : PolicyHandler<ManagedIdentityAuthenticationConfig>
{
    public List<Tuple<
            Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool>,
            Func<string, string?, string>
    >> ProvideTokenHooks { get; } = new();

    public override string PolicyName => nameof(IInboundContext.AuthenticationManagedIdentity);

    protected override void Handle(GatewayContext context, ManagedIdentityAuthenticationConfig config)
    {
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