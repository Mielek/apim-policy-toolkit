using System.Collections.Immutable;
using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

public partial class PolicyFragmentBuilder : PolicySectionBuilder
{
    public PolicyFragmentBuilder() { }

    public XElement Create()
    {
        return new XElement("fragment", _sectionPolicies.ToArray());
    }
}