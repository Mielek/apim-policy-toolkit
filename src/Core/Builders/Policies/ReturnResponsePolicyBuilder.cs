using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class ReturnResponsePolicyBuilder : BaseBuilder<ReturnResponsePolicyBuilder>
{
    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _setHeaderPolicies;
    [IgnoreBuilderField]
    private XElement? _setBodyPolicy;
    [IgnoreBuilderField]
    private XElement? _setStatusPolicy;
    private string? _responseVariableName;

    public ReturnResponsePolicyBuilder SetHeader(Action<SetHeaderPolicyBuilder> configuration)
    {
        var builder = new SetHeaderPolicyBuilder();
        configuration(builder);
        (_setHeaderPolicies ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }
    public ReturnResponsePolicyBuilder SetBody(Action<SetBodyPolicyBuilder> configuration)
    {
        var builder = new SetBodyPolicyBuilder();
        configuration(builder);
        _setBodyPolicy = builder.Build();
        return this;
    }
    public ReturnResponsePolicyBuilder SetStatus(Action<SetStatusPolicyBuilder> configuration)
    {
        var builder = new SetStatusPolicyBuilder();
        configuration(builder);
        _setStatusPolicy = builder.Build();
        return this;
    }

    public XElement Build()
    {
        var element = CreateElement("return-response");
        if (_responseVariableName != null)
        {
            element.Add(new XAttribute("response-variable-name", _responseVariableName));
        }
        if (_setStatusPolicy != null)
        {
            element.Add(_setStatusPolicy);
        }
        if (_setHeaderPolicies != null && _setHeaderPolicies.Count > 0)
        {
            element.Add(_setHeaderPolicies.ToImmutable());
        }
        if (_setBodyPolicy != null)
        {
            element.Add(_setBodyPolicy);
        }

        return element;
    }
}