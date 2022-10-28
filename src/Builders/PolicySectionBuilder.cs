using System.Collections.Immutable;

using Mielek.Model.Policies;

namespace Mielek.Builders;
public partial class PolicySectionBuilder
{
    protected ImmutableList<IPolicy>.Builder sectionPolicies = ImmutableList.CreateBuilder<IPolicy>();

    internal PolicySectionBuilder() { }

    internal ICollection<IPolicy> Build()
    {
        return sectionPolicies.ToImmutable();
    }
}