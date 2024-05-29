using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class ChoosePolicyBuilder<TSectionBuilder> : BaseBuilder<ChoosePolicyBuilder<TSectionBuilder>> where TSectionBuilder : PolicySectionBuilder, new()
{
    private readonly ImmutableList<XElement>.Builder _whens = ImmutableList.CreateBuilder<XElement>();
    private ICollection<XElement>? _otherwise;

    public ChoosePolicyBuilder<TSectionBuilder> When(Action<ChooseWhenBuilder<TSectionBuilder>> configurator)
    {
        var builder = new ChooseWhenBuilder<TSectionBuilder>();
        configurator(builder);
        _whens.Add(builder.Build());
        return this;
    }

    public ChoosePolicyBuilder<TSectionBuilder> Otherwise(Action<TSectionBuilder> configurator)
    {
        var builder = new TSectionBuilder();
        configurator(builder);
        _otherwise = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_whens.Count == 0) throw new PolicyValidationException("At least one When is required for Choose");

        var element = this.CreateElement("choose");

        element.Add(_whens.ToArray());

        if (_otherwise != null && _otherwise.Count > 0)
        {
            element.Add(new XElement("otherwise", _otherwise.ToArray()));
        }

        return element;
    }
}

[GenerateBuilderSetters]
public partial class ChooseWhenBuilder<TSectionBuilder> where TSectionBuilder : PolicySectionBuilder, new()
{
    private readonly IExpression<bool>? _condition;

    [IgnoreBuilderField]
    private ICollection<XElement>? _policies;

    public ChooseWhenBuilder<TSectionBuilder> Policies(Action<TSectionBuilder> configurator)
    {
        var builder = new TSectionBuilder();
        configurator(builder);
        _policies = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_condition == null) throw new PolicyValidationException("Condition is required for When");
        if (_policies == null) throw new PolicyValidationException("Policies are required for When");

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(_condition.GetXAttribute("condition"));
        children.AddRange(_policies.ToArray());

        return new XElement("when", children.ToArray());
    }
}