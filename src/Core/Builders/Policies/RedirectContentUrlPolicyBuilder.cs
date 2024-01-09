namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

using System.Xml.Linq;

public partial class InboundSectionBuilder
{
    public PolicySectionBuilder RedirectContentUrls()
    {
        _sectionPolicies.Add(new XElement("redirect-content-urls"));
        return this;
    }
}

public partial class OutboundSectionBuilder
{
    public PolicySectionBuilder RedirectContentUrls()
    {
        _sectionPolicies.Add(new XElement("redirect-content-urls"));
        return this;
    }
}

public partial class PolicyFragmentBuilder
{
    public PolicySectionBuilder RedirectContentUrls()
    {
        _sectionPolicies.Add(new XElement("redirect-content-urls"));
        return this;
    }
}