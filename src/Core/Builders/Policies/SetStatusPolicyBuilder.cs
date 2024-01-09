namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class SetStatusPolicyBuilder
{
    private IExpression<string>? _code;
    private IExpression<string>? _reason;

    public SetStatusPolicyBuilder Code(ushort code)
    {
        return Code($"{code}");
    }

    public XElement Build()
    {
        if (_code == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(_code.GetXAttribute("code"));
        if (_reason != null)
        {
            children.Add(_reason.GetXAttribute("reason"));
        }
        return new XElement("set-status", children.ToArray());
    }
}