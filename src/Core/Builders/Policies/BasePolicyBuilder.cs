using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

public partial class InboundSectionBuilder
{
    public InboundSectionBuilder Base()
    {
        _sectionPolicies.Add(new XElement("base"));
        return this;
    }
}

public partial class OutboundSectionBuilder
{
    public OutboundSectionBuilder Base()
    {
        _sectionPolicies.Add(new XElement("base"));
        return this;
    }
}

public partial class BackendSectionBuilder
{
    public BackendSectionBuilder Base()
    {
        _sectionPolicies.Add(new XElement("base"));
        return this;
    }
}

public partial class OnErrorSectionBuilder
{
    public OnErrorSectionBuilder Base()
    {
        _sectionPolicies.Add(new XElement("base"));
        return this;
    }
}