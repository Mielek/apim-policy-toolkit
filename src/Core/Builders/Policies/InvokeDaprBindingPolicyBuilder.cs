using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Core.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder))
]
public partial class InvokeDaprBindingPolicyBuilder : BaseBuilder<InvokeDaprBindingPolicyBuilder>
{
    private readonly IExpression<string>? _name;

    public XElement Build()
    {
        var element = this.CreateElement("invoke-dapr-binding");
        return element;
    }
}