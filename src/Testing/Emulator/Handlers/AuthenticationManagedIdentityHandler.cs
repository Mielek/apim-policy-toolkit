// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Handlers;

[Section(nameof(IInboundContext))]
public class AuthenticationManagedIdentityHandler : IInvokeHandler
{
    public Action<MockExpressionContext, ManagedIdentityAuthenticationConfig>? Interceptor { private get; init; }
    public Func<string, string?, string>? TokenProvider { private get; init; }

    public string MethodName => nameof(IInboundContext.AuthenticationManagedIdentity);

    public object? Invoke(GatewayContext context, object?[]? args)
    {
        if (args is not { Length: 1 })
        {
            throw new ArgumentException("Expected 1 argument", nameof(args));
        }

        if (args[0] is not ManagedIdentityAuthenticationConfig config)
        {
            throw new ArgumentException("Expected ManagedIdentityAuthenticationConfig as first argument", nameof(args));
        }

        if (Interceptor is not null)
        {
            Interceptor(context.RuntimeContext, config);
            return null;
        }

        var token = CreateTokenByProvider(config) ?? CreateInternalToken(config);

        if (!string.IsNullOrWhiteSpace(config.OutputTokenVariableName))
        {
            context.RuntimeContext.Variables[config.OutputTokenVariableName] = token;
        }
        else
        {
            context.RuntimeContext.Request.Headers["Authorization"] = [$"Bearer {token}"];
        }

        return null;
    }

    private string CreateInternalToken(ManagedIdentityAuthenticationConfig config)
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

    private string? CreateTokenByProvider(ManagedIdentityAuthenticationConfig config)
    {
        if (TokenProvider is null)
        {
            return null;
        }

        string token;
        try
        {
            token = TokenProvider(config.Resource, config.ClientId);
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