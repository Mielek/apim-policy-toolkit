using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

public partial class InboundSectionBuilder
{
    public PolicySectionBuilder RedirectContentUrls(string? id = null)
    {
        var element = new XElement("redirect-content-urls");
        if (id != null)
        {
            element.Add(new XAttribute("id", id));
        }
        _sectionPolicies.Add(new XElement("redirect-content-urls"));
        return this;
    }
}

public partial class OutboundSectionBuilder
{
    public PolicySectionBuilder RedirectContentUrls(string? id = null)
    {
        var element = new XElement("redirect-content-urls");
        if (id != null)
        {
            element.Add(new XAttribute("id", id));
        }
        _sectionPolicies.Add(new XElement("redirect-content-urls"));
        return this;
    }
}

public partial class PolicyFragmentBuilder
{
    public PolicySectionBuilder RedirectContentUrls(string? id = null)
    {
        var element = new XElement("redirect-content-urls");
        if (id != null)
        {
            element.Add(new XAttribute("id", id));
        }
        _sectionPolicies.Add(new XElement("redirect-content-urls"));
        return this;
    }
}