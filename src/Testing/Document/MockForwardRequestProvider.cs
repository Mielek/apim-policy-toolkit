using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockForwardRequestProvider
{
    public static MockForwardRequest ForwardRequest(this MockPoliciesProvider<IBackendContext> mock) =>
        ForwardRequest(mock, (_, _) => true);

    public static MockForwardRequest ForwardRequest<T>(
        this MockPoliciesProvider<T> mock,
        Func<GatewayContext, ForwardRequestConfig?, bool> predicate
    ) where T : class
    {
        var handler = mock.SectionContextProxy.GetHandler<ForwardRequestHandler>();
        return new MockForwardRequest(predicate, handler);
    }

    public class MockForwardRequest
    {
        private Func<GatewayContext, ForwardRequestConfig?, bool> _predicate;
        private ForwardRequestHandler _handler;

        internal MockForwardRequest(
            Func<GatewayContext, ForwardRequestConfig?, bool> predicate,
            ForwardRequestHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext, ForwardRequestConfig?> callback) => _handler.CallbackHooks.Add(
            new Tuple<Func<GatewayContext, ForwardRequestConfig?, bool>, Action<GatewayContext, ForwardRequestConfig?>>(
                _predicate, callback));
    }
}