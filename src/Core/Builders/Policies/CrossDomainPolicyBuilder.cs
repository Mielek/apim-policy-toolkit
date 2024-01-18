using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class CrossDomainPolicyBuilder : BaseBuilder<CrossDomainPolicyBuilder>
{

    private ImmutableList<XElement>.Builder? _policy;

    public CrossDomainPolicyBuilder Policy(XElement element)
    {
        (_policy ??= ImmutableList.CreateBuilder<XElement>()).Add(element);
        return this;
    }


    public XElement Build()
    {
        if (_policy == null || _policy.Count == 0) throw new PolicyValidationException("At least one Policy is required for CrossDomain");

        var element = this.CreateElement("cross-domain");
        element.Add(_policy.ToArray());
        return element;
    }
}