namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class CrossDomainPolicyBuilder
{

    private ImmutableList<XElement>.Builder? _policy;

    CrossDomainPolicyBuilder Policy(XElement element)
    {
        (_policy ??= ImmutableList.CreateBuilder<XElement>()).Add(element);
        return this;
    }


    public XElement Build()
    {
        if (_policy == null || _policy.Count == 0) throw new NullReferenceException();

        return new XElement("cross-domain", _policy.ToArray());
    }
}