using System.Xml;
using System.Xml.Linq;

using Mielek.Model;


namespace Mielek.Builders;
public class PolicyDocumentBuilder
{
    public static PolicyDocumentBuilder Create() => new();

    private ICollection<XElement>? _inbound;
    private ICollection<XElement>? _backend;
    private ICollection<XElement>? _outbound;
    private ICollection<XElement>? _onError;

    private PolicyDocumentBuilder() { }

    public PolicyDocumentBuilder Inbound(Action<PolicySectionBuilder> configurator)
    {
        _inbound = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder Backend(Action<PolicySectionBuilder> configurator)
    {
        _backend = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder Outbound(Action<PolicySectionBuilder> configurator)
    {
        _outbound = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder OnError(Action<PolicySectionBuilder> configurator)
    {
        _onError = BuildSection(configurator);
        return this;
    }

    private ICollection<XElement> BuildSection(Action<PolicySectionBuilder> configurator)
    {
        var builder = new PolicySectionBuilder();
        configurator(builder);
        return builder.Build();
    }

    public XElement Build()
    {
        var document = new XElement("policies");
        document.Add(new XElement("inbound", _inbound?.ToArray()));
        document.Add(new XElement("backend", _backend?.ToArray()));
        document.Add(new XElement("outbound", _outbound?.ToArray()));
        document.Add(new XElement("on-error", _onError?.ToArray()));
        return document;
    }
}