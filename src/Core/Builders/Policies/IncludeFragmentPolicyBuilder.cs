namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder))
]
public partial class IncludeFragmentPolicyBuilder
{
    private string? _fragmentId;
    public XElement Build()
    {
        if (_fragmentId == null) throw new NullReferenceException();

        return new XElement("include-fragment", new XAttribute("fragment-id", _fragmentId));
    }
}
