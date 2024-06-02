using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder))
]
public partial class IncludeFragmentPolicyBuilder : BaseBuilder<IncludeFragmentPolicyBuilder>
{
    private string? _fragmentId;
    public XElement Build()
    {
        if (_fragmentId == null) throw new PolicyValidationException("FragmentId is required for IncludeFragment");
        var element = this.CreateElement("include-fragment");
        element.Add(new XAttribute("fragment-id", _fragmentId));
        return element;
    }
}