// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockCorsProvider
{
    public static Setup Cors(this MockPoliciesProvider<IInboundContext> mock) =>
        Cors(mock, (_, _) => true);

    public static Setup Cors(
        this MockPoliciesProvider<IInboundContext> mock,
        Func<GatewayContext, CorsConfig, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<CorsHandler>();
        return new Setup(predicate, handler);
    }

    public class Setup
    {
        private readonly Func<GatewayContext, CorsConfig, bool> _predicate;
        private readonly CorsHandler _handler;

        internal Setup(
            Func<GatewayContext, CorsConfig, bool> predicate,
            CorsHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext, CorsConfig> callback) =>
            _handler.CallbackSetup.Add((_predicate, callback).ToTuple());
    }
}