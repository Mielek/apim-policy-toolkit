using System.Collections.Immutable;

using Mielek.Model;

namespace Mielek.Builders;
public class PolicyFragmentBuilder
{
    public static PolicyFragmentBuilder Create()
    {
        return new PolicyFragmentBuilder();
    }

    readonly PolicySectionBuilder _innerBuilder = new PolicySectionBuilder();

    PolicyFragmentBuilder() { }

    public PolicyFragmentBuilder Policies(Action<PolicySectionBuilder> configurator)
    {
        configurator(_innerBuilder);
        return this;
    }

    public PolicyFragment Build()
    {
        return new PolicyFragment(_innerBuilder.Build());
    }
}