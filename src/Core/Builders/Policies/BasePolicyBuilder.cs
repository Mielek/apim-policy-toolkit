using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

public partial class PolicySectionBuilder
{
    public PolicySectionBuilder Base()
    {
        _sectionPolicies.Add(new XElement("base"));
        return this;
    }
}