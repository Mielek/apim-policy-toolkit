using System.Collections.Immutable;
using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

public abstract class PolicySectionBuilder
{
    protected ImmutableList<XElement>.Builder _sectionPolicies = ImmutableList.CreateBuilder<XElement>();

    internal ICollection<XElement> Build()
    {
        return _sectionPolicies.ToImmutable();
    }
}

public partial class InboundSectionBuilder : PolicySectionBuilder { };
public partial class BackendSectionBuilder : PolicySectionBuilder { };
public partial class OutboundSectionBuilder : PolicySectionBuilder { };
public partial class OnErrorSectionBuilder : PolicySectionBuilder { };