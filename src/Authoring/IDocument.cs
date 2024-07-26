namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IDocument
{
    void Inbound(IInboundContext section) { }
    void Outbound(IOutboundContext section) { }
    void Backend(IBackendContext section) { }
    void OnError(IOnErrorContext section) { }
}