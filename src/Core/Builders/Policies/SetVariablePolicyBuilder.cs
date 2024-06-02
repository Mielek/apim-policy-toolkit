using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
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
public partial class SetVariablePolicyBuilder : BaseBuilder<SetVariablePolicyBuilder>
{
    private string? _name;
    private IExpression<string>? _value;

    public XElement Build()
    {
        if (_name == null) throw new PolicyValidationException("Name is required for SetVariable");
        if (_value == null) throw new PolicyValidationException("Value is required for SetVariable");

        var element = CreateElement("set-variable");
        element.Add(
            new XAttribute("name", _name),
            _value.GetXAttribute("value")
        );
        return element;
    }
}