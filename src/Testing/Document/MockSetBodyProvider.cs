// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockSetBodyProvider
{
    public static MockSetBody SetBody(this MockPoliciesProvider<IInboundContext> mock) =>
        SetBody(mock, (_, _, _) => true);

    public static MockSetBody SetBody(this MockPoliciesProvider<IOutboundContext> mock) =>
        SetBody(mock, (_, _, _) => true);

    public static MockSetBody SetBody(this MockPoliciesProvider<IOnErrorContext> mock) =>
        SetBody(mock, (_, _, _) => true);

    public static MockSetBody SetBody(
        this MockPoliciesProvider<IInboundContext> mock,
        Func<GatewayContext, string, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetBodyRequestHandler>();
        return new MockSetBody(predicate, handler);
    }

    public static MockSetBody SetBody(
        this MockPoliciesProvider<IOutboundContext> mock,
        Func<GatewayContext, string, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetBodyResponseHandler>();
        return new MockSetBody(predicate, handler);
    }

    public static MockSetBody SetBody(
        this MockPoliciesProvider<IOnErrorContext> mock,
        Func<GatewayContext, string, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetBodyResponseHandler>();
        return new MockSetBody(predicate, handler);
    }

    public class MockSetBody
    {
        private readonly Func<GatewayContext, string, SetBodyConfig?, bool> _predicate;
        private readonly SetBodyHandler _handler;

        internal MockSetBody(
            Func<GatewayContext, string, SetBodyConfig?, bool> predicate,
            SetBodyHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext, string, SetBodyConfig?> callback) => _handler.CallbackHooks.Add(
            new Tuple<Func<GatewayContext, string, SetBodyConfig?, bool>, Action<GatewayContext, string, SetBodyConfig?>>(
                _predicate, callback));
    }
}