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
        Func<GatewayContext, object, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetHeaderRequestHandler>();
        return new MockSetHeader(predicate, handler);
    }

    public static MockSetHeader SetHeader(
        this MockPoliciesProvider<IOutboundContext> mock,
        Func<GatewayContext, object, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetHeaderResponseHandler>();
        return new MockSetHeader(predicate, handler);
    }

    public static MockSetHeader SetHeader(
        this MockPoliciesProvider<IOnErrorContext> mock,
        Func<GatewayContext, object, SetBodyConfig?, bool> predicate
    )
    {
        var handler = mock.SectionContextProxy.GetHandler<SetHeaderResponseHandler>();
        return new MockSetHeader(predicate, handler);
    }
    
    public class MockSetHeader
    {
        private Func<GatewayContext, object, SetBodyConfig?, bool> _predicate;
        private SetHeaderHandler _handler;

        internal MockSetHeader(
            Func<GatewayContext, object, SetBodyConfig?, bool> predicate,
            SetHeaderHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }
    }
}