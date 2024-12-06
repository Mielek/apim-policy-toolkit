// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Text;

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
        const string header = "{ \"alg\": \"RS256\",\"typ\": \"JWT\" }";
        var headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
        var payload = $"{{ \"resource\"=\"{config.Resource}\"";

        if (!string.IsNullOrWhiteSpace(config.ClientId))
        {
            payload += $" \"client_id\": \"{config.ClientId}\"";
        }

        if (config.IgnoreError is not null)
        {
            payload += $" \"ignore_error\": \"{config.IgnoreError}\"";
        }
        payload += " }";
        var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));

        var signature = $"{headerBase64}.{payloadBase64}";
        var signatureBytes = Encoding.UTF8.GetBytes(signature);
        byte[] secretKey = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(secretKey);
        using var hmac = new HMACSHA256(secretKey);
        var hash = hmac.ComputeHash(signatureBytes);
        var hashBase64 = Convert.ToBase64String(hash);

        return $"{headerBase64}.{payloadBase64}.{hashBase64}";
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