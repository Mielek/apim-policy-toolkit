using System.Collections.Immutable;
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
public partial class SetHeaderPolicyBuilder : BaseBuilder<SetHeaderPolicyBuilder>
{
    public enum ExistsActionType { Override, Skip, Append, Delete }

    private IExpression<string>? _name;
    private ImmutableList<IExpression<string>>.Builder? _values;
    private IExpression<string>? _existsAction;

    public SetHeaderPolicyBuilder ExistsAction(ExistsActionType existsAction)
    {
        return ExistsAction(Translate(existsAction));
    }

    private string Translate(ExistsActionType existsAction) => existsAction switch
    {
        ExistsActionType.Override => "override",
        ExistsActionType.Append => "append",
        ExistsActionType.Delete => "delete",
        ExistsActionType.Skip => "skip",
        _ => throw new PolicyValidationException("Unknown exists action for SetHeader policy"),
    };

    public XElement Build()
    {
        if (_name == null) throw new PolicyValidationException("SetHeader requires name");

        var element = CreateElement("set-header");
        element.Add(_name.GetXAttribute("name"));
        if (_existsAction != null)
        {
            element.Add(_existsAction.GetXAttribute("exists-action"));
        }
        if (_values != null && _values.Count > 0)
        {
            element.Add(_values.ToImmutable().Select(v => new XElement("value", v.GetXText())));
        }

        return element;
    }
}