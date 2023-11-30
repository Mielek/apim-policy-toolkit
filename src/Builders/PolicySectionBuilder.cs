using System.Collections.Immutable;
using System.Xml.Linq;



namespace Mielek.Builders;
public partial class PolicySectionBuilder
{
    protected ImmutableList<XElement>.Builder _sectionPolicies = ImmutableList.CreateBuilder<XElement>();

    internal PolicySectionBuilder() { }

    internal ICollection<XElement> Build()
    {
        return _sectionPolicies.ToImmutable();
    }
}