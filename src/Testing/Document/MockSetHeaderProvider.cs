// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockSetHeaderProvider
{
    public static MockSetHeader SetHeader(this MockPoliciesProvider<IInboundContext> mock) =>
        SetHeader(mock, (_, _, _) => true);

    public static MockSetHeader SetHeader(this MockPoliciesProvider<IOutboundContext> mock) =>
        SetHeader(mock, (_, _, _) => true);

    public static MockSetHeader SetHeader(this MockPoliciesProvider<IOnErrorContext> mock) =>
        SetHeader(mock, (_, _, _) => true);

    public static MockSetHeader SetHeader(
        this MockPoliciesProvider<IInboundContext> mock,
        Func<GatewayContext, string, string[], bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetHeaderRequestHandler>();
        return new MockSetHeader(predicate, handler);
    }

    public static MockSetHeader SetHeader(
        this MockPoliciesProvider<IOutboundContext> mock,
        Func<GatewayContext, string, string[], bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetHeaderResponseHandler>();
        return new MockSetHeader(predicate, handler);
    }

    public static MockSetHeader SetHeader(
        this MockPoliciesProvider<IOnErrorContext> mock,
        Func<GatewayContext, string, string[], bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetHeaderResponseHandler>();
        return new MockSetHeader(predicate, handler);
    }
    
    public class MockSetHeader
    {
        private readonly Func<GatewayContext, string, string[], bool> _predicate;
        private readonly SetHeaderHandler _handler;

        internal MockSetHeader(
            Func<GatewayContext, string, string[], bool> predicate,
            SetHeaderHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext, string, string[]> callback) => _handler.CallbackHooks.Add(
            new Tuple<Func<GatewayContext, string, string[], bool>, Action<GatewayContext, string, string[]>>(
                _predicate, callback));
    }
}