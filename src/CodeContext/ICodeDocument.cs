namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

public interface ICodeDocument
{
    void Inbound(IInboundContext c) { }
    void Outbound(IOutboundContext c) { }
    void Backend(IBackendContext c) { }
    void OnError(IOnErrorContext c) { }
}