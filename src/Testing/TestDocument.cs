using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

namespace Azure.ApiManagement.PolicyToolkit.Testing;

public class TestDocument(IDocument document)
{
    public GatewayContext Context { get; set; } = new();

    public void RunInbound() => document.Inbound(InboundPolicies.SectionContext);
    public void RunBackend() => document.Backend(BackendPolicies.SectionContext);
    public void RunOutbound() => document.Outbound(OutboundPolicies.SectionContext);
    public void RunOnError() => document.OnError(OnErrorPolicies.SectionContext);
    
    public TestPoliciesProvider<IInboundContext> InboundPolicies => new(Context);
    public TestPoliciesProvider<IBackendContext> BackendPolicies => new(Context);
    public TestPoliciesProvider<IOutboundContext> OutboundPolicies => new(Context);
    public TestPoliciesProvider<IOnErrorContext> OnErrorPolicies => new(Context);
}