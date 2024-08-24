using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
public class PolicyDocumentBuilder
{
    private ICollection<XElement>? _inbound;
    private ICollection<XElement>? _backend;
    private ICollection<XElement>? _outbound;
    private ICollection<XElement>? _onError;

    public PolicyDocumentBuilder() { }

    public PolicyDocumentBuilder Inbound(Action<InboundSectionBuilder> configurator)
    {
        _inbound = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder Backend(Action<BackendSectionBuilder> configurator)
    {
        _backend = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder Outbound(Action<OutboundSectionBuilder> configurator)
    {
        _outbound = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder OnError(Action<OnErrorSectionBuilder> configurator)
    {
        _onError = BuildSection(configurator);
        return this;
    }

    private ICollection<XElement> BuildSection<Builder>(Action<Builder> configurator) where Builder : PolicySectionBuilder, new()
    {
        var builder = new Builder();
        configurator(builder);
        return builder.Build();
    }

    public XElement Create()
    {
        var document = new XElement("policies");
        if (_inbound != null && _inbound.Count > 0)
        {
            document.Add(new XElement("inbound", _inbound.ToArray()));
        }
        if (_backend != null && _backend.Count > 0)
        {
            document.Add(new XElement("backend", _backend.ToArray()));
        }
        if (_outbound != null && _outbound.Count > 0)
        {
            document.Add(new XElement("outbound", _outbound.ToArray()));
        }
        if (_onError != null && _onError.Count > 0)
        {
            document.Add(new XElement("on-error", _onError.ToArray()));
        }
        return document;
    }
}