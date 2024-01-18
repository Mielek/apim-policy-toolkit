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
public partial class SetStatusPolicyBuilder : BaseBuilder<SetStatusPolicyBuilder>
{
    private IExpression<string>? _code;
    private IExpression<string>? _reason;

    public SetStatusPolicyBuilder Code(ushort code)
    {
        return Code($"{code}");
    }

    public XElement Build()
    {
        if (_code == null) throw new PolicyValidationException("Code is required for SetStatus");

        var element = this.CreateElement("set-status");
        element.Add(_code.GetXAttribute("code"));
        if (_reason != null)
        {
            element.Add(_reason.GetXAttribute("reason"));
        }
        return element;
    }
}