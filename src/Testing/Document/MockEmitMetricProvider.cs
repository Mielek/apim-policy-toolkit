// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockEmitMetricProvider
{
    public static Setup EmitMetric<T>(this MockPoliciesProvider<T> mock) where T : class =>
        EmitMetric(mock, (_, _) => true);

    public static Setup EmitMetric<T>(
        this MockPoliciesProvider<T> mock,
        Func<GatewayContext, EmitMetricConfig, bool> predicate
    ) where T : class
    {
        var handler = mock.SectionContextProxy.GetHandler<EmitMetricHandler>();
        return new Setup(predicate, handler);
    }

    public class Setup
    {
        private readonly Func<GatewayContext, EmitMetricConfig, bool> _predicate;
        private readonly EmitMetricHandler _handler;

        internal Setup(
            Func<GatewayContext, EmitMetricConfig, bool> predicate,
            EmitMetricHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext, EmitMetricConfig> callback) =>
            _handler.CallbackHooks.Add((_predicate, callback).ToTuple());
    }
}