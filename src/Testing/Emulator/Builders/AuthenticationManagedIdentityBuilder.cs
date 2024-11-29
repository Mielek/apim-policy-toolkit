using System.Security.Cryptography;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Builders;

public class AuthenticationManagedIdentityBuilder
{
    private Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool> _predicate;
    private AuthenticationManagedIdentityHandler _handler;

    internal AuthenticationManagedIdentityBuilder(
        Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool> predicate,
        AuthenticationManagedIdentityHandler handler)
    {
        _predicate = predicate;
        _handler = handler;
    }

    public void WithCallback(Action<GatewayContext, ManagedIdentityAuthenticationConfig> callback)
        => _handler.CallbackHooks.Add(new Tuple<Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool>, Action<GatewayContext, ManagedIdentityAuthenticationConfig>>(_predicate, callback));

    public void WithTokenProviderHook(Func<string, string?, string> hook)
        => _handler.ProvideTokenHooks.Add(new Tuple<Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool>, Func<string, string?, string>>(_predicate, hook));

    public void ReturnsToken(string token) => this.WithTokenProviderHook((_, _) => token);
    public void WithError(string error) => this.WithTokenProviderHook((_, _) => throw new InvalidOperationException(error));
}

public static class AuthenticationManagedIdentityBuilderExtensions
{
    public static AuthenticationManagedIdentityBuilder AuthenticationManagedIdentity(this TestPoliciesProvider<IInboundContext> test)
        => AuthenticationManagedIdentity(test, (_, _) => true);

    public static AuthenticationManagedIdentityBuilder AuthenticationManagedIdentity(
        this TestPoliciesProvider<IInboundContext> test,
        Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool> predicate)
    {
        var handler = test.SectionContextProxy.GetHandler<AuthenticationManagedIdentityHandler>();
        return new AuthenticationManagedIdentityBuilder(predicate, handler);
    }
}