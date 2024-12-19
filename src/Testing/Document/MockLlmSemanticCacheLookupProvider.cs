// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockLlmSemanticCacheLookupProvider
{
    public static Setup LlmSemanticCacheLookup(
        this MockPoliciesProvider<IInboundContext> mock) => LlmSemanticCacheLookup(mock, (_, _) => true);

    public static Setup LlmSemanticCacheLookup(
        this MockPoliciesProvider<IInboundContext> mock,
        Func<GatewayContext, SemanticCacheLookupConfig, bool> predicate)
    {
        var handler = mock.SectionContextProxy.GetHandler<LlmSemanticCacheLookupHandler>();
        return new Setup(predicate, handler);
    }

    public class Setup
    {
        private readonly Func<GatewayContext, SemanticCacheLookupConfig, bool> _predicate;
        private readonly LlmSemanticCacheLookupHandler _handler;

        internal Setup(
            Func<GatewayContext, SemanticCacheLookupConfig, bool> predicate,
            LlmSemanticCacheLookupHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext, SemanticCacheLookupConfig> callback) =>
            _handler.CallbackHooks.Add((_predicate, callback).ToTuple());
    }
}