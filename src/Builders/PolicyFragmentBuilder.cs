using Mielek.Model;

namespace Mielek.Builders;
public class PolicyFragmentBuilder
{
    public static PolicyFragmentBuilder Create() => new();

    readonly PolicySectionBuilder _innerBuilder = new();

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