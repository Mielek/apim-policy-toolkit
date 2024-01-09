namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class MockResponsePolicyBuilder
{
    private uint? _statusCode;
    private string? _contentType;

    public XElement Build()
    {
        var children = ImmutableArray.CreateBuilder<object>();

        if (_statusCode != null)
        {
            children.Add(new XAttribute("status-code", _statusCode));
        }

        if (_contentType != null)
        {
            children.Add(new XAttribute("content-type", _contentType));
        }

        return new XElement("mock-response", children.ToArray());
    }
}