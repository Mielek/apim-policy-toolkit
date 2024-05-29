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
public partial class FindAndReplacePolicyBuilder : BaseBuilder<FindAndReplacePolicyBuilder>
{
    private readonly IExpression<string>? _from;
    private readonly IExpression<string>? _to;

    public XElement Build()
    {
        if (_from == null) throw new PolicyValidationException("From is required for FindAndReplace");
        if (_to == null) throw new PolicyValidationException("To is required for FindAndReplace");

        var element = this.CreateElement("find-and-replace");

        element.Add(_from.GetXAttribute("from"));
        element.Add(_to.GetXAttribute("to"));

        return element;
    }
}