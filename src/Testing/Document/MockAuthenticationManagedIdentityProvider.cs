// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockAuthenticationManagedIdentityProvider
{
    public static Setup AuthenticationManagedIdentity(
        this MockPoliciesProvider<IInboundContext> mock) => AuthenticationManagedIdentity(mock, (_, _) => true);

    public static Setup AuthenticationManagedIdentity(
        this MockPoliciesProvider<IInboundContext> mock,
        Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool> predicate)
    {
        var handler = mock.SectionContextProxy.GetHandler<AuthenticationManagedIdentityHandler>();
        return new Setup(predicate, handler);
    }

    public class Setup
    {
        private readonly Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool> _predicate;
        private readonly AuthenticationManagedIdentityHandler _handler;

        internal Setup(
            Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool> predicate,
            AuthenticationManagedIdentityHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext, ManagedIdentityAuthenticationConfig> callback) =>
            _handler.CallbackHooks.Add(
                new Tuple<Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool>,
                    Action<GatewayContext, ManagedIdentityAuthenticationConfig>>(_predicate, callback));

        public void WithTokenProviderHook(Func<string, string?, string> hook) => _handler.ProvideTokenHooks.Add(
            new Tuple<Func<GatewayContext, ManagedIdentityAuthenticationConfig, bool>, Func<string, string?, string>>(
                _predicate, hook));

        public void ReturnsToken(string token) => this.WithTokenProviderHook((_, _) => token);

        public void WithError(string error) =>
            this.WithTokenProviderHook((_, _) => throw new InvalidOperationException(error));
    }
}