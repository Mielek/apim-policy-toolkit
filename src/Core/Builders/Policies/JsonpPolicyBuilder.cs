namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class JsonpPolicyBuilder
{
    private IExpression<string>? _callbackParameterName;

    public XElement Build()
    {
        if(_callbackParameterName == null) throw new NullReferenceException();

        return new XElement("jsonp", _callbackParameterName.GetXAttribute("callback-parameter-name"));
    }
}