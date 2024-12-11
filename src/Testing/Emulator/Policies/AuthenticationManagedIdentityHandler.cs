// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IdentityModel.Tokens.Jwt;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

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
            : DefaultTokenProvider(config);

        if (!string.IsNullOrWhiteSpace(config.OutputTokenVariableName))
        {
            context.Variables[config.OutputTokenVariableName] = token;
        }
        else
        {
            context.Request.Headers["Authorization"] = [$"Bearer {token}"];
        }
    }

    private string DefaultTokenProvider(ManagedIdentityAuthenticationConfig config)
    {
        byte[] securityKey = JwtTokenUtilities.GenerateKeyBytes(256);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
            issuer: "mock",
            audience: config.Resource,
            signingCredentials: credentials);

        token.Payload["resource"] = config.Resource;
        if (!string.IsNullOrWhiteSpace(config.ClientId))
        {
            token.Payload["client_id"] = config.ClientId;
        }
        if (config.IgnoreError is not null)
        {
            token.Payload["ignore_error"] = config.IgnoreError;
        }

        return new JwtSecurityTokenHandler().WriteToken(token);
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