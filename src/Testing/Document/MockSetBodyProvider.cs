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
        Func<GatewayContext, object, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetBodyRequestHandler>();
        return new MockSetBody(predicate, handler);
    }

    public static MockSetBody SetBody(
        this MockPoliciesProvider<IOutboundContext> mock,
        Func<GatewayContext, object, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetBodyResponseHandler>();
        return new MockSetBody(predicate, handler);
    }

    public static MockSetBody SetBody(
        this MockPoliciesProvider<IOnErrorContext> mock,
        Func<GatewayContext, object, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetBodyResponseHandler>();
        return new MockSetBody(predicate, handler);
    }

    public class MockSetBody
    {
        private Func<GatewayContext, object, SetBodyConfig?, bool> _predicate;
        private SetBodyHandler _handler;

        internal MockSetBody(
            Func<GatewayContext, object, SetBodyConfig?, bool> predicate,
            SetBodyHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }
    }
}