using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class JsonpPolicyBuilder : BaseBuilder<JsonpPolicyBuilder>
{
    private IExpression<string>? _callbackParameterName;

    public XElement Build()
    {
        if (_callbackParameterName == null) throw new PolicyValidationException("CallbackParameterName is required for Jsonp");
        var element = this.CreateElement("jsonp");
        element.Add(_callbackParameterName.GetXAttribute("callback-parameter-name"));
        return element;
    }
}