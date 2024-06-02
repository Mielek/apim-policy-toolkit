namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

public interface ICodeDocument
{
    void Inbound(IInboundContext section) { }
    void Outbound(IOutboundContext section) { }
    void Backend(IBackendContext section) { }
    void OnError(IOnErrorContext section) { }
}