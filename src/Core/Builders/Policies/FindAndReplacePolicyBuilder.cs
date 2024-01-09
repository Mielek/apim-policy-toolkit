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
public partial class FindAndReplacePolicyBuilder
{
    private IExpression<string>? _from;
    private IExpression<string>? _to;

    public XElement Build()
    {
        if (_from == null) throw new NullReferenceException();
        if (_to == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<XObject>();

        children.Add(_from.GetXAttribute("from"));
        children.Add(_to.GetXAttribute("to"));

        return new XElement("find-and-replace", children.ToArray());
    }
}