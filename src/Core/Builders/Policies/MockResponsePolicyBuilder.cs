using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class MockResponsePolicyBuilder : BaseBuilder<MockResponsePolicyBuilder>
{
    private uint? _statusCode;
    private string? _contentType;

    public XElement Build()
    {
        var element = CreateElement("mock-response");

        if (_statusCode != null)
        {
            element.Add(new XAttribute("status-code", _statusCode));
        }

        if (_contentType != null)
        {
            element.Add(new XAttribute("content-type", _contentType));
        }

        return element;
    }
}