using System.Xml.Linq;

namespace Mielek.Builders;

public partial class PolicySectionBuilder
{
    public PolicySectionBuilder Base()
    {
        _sectionPolicies.Add(new XElement("base"));
        return this;
    }
}