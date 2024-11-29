// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
public class AuthenticationBasicHandler : IPolicyHandler
{
    public Action<MockExpressionContext, string, string>? Interceptor { private get; init; }

    public string PolicyName => nameof(IInboundContext.AuthenticationBasic);

    public object? Handle(GatewayContext context, object?[]? args)
    {
        if (args is not { Length: 2 })
        {
            throw new ArgumentException("Invalid number of arguments");
        }

        if (args[0] is not string username || args[1] is not string password)
        {
            throw new ArgumentException("Invalid argument type");
        }

        if (Interceptor is not null)
        {
            Interceptor(context, username, password);
        }
        else
        {
            var authHeader = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"))}";
            context.Request.Headers["Authorization"] = [authHeader];
        }

        return null;
    }
}