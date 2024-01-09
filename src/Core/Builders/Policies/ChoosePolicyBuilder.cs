namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class ChoosePolicyBuilder<TSectionBuilder> where TSectionBuilder : PolicySectionBuilder, new()
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

    public ChoosePolicyBuilder<TSectionBuilder> Otherwise(Action<PolicySectionBuilder> configurator)
    {
        var builder = new TSectionBuilder();
        configurator(builder);
        _otherwise = builder.Build();
        return this;
    }

    public XElement Build()
    {
        var children = ImmutableArray.CreateBuilder<object>();

        children.Add(_whens.ToArray());

        if (_otherwise != null && _otherwise.Count > 0)
        {
            children.Add(new XElement("otherwise", _otherwise.ToArray()));
        }

        return new XElement("choose", children.ToArray());
    }
}

[GenerateBuilderSetters]
public partial class ChooseWhenBuilder<TSectionBuilder> where TSectionBuilder : PolicySectionBuilder, new()
{
    private IExpression<bool>? _condition;

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
        if (_condition == null) throw new NullReferenceException();
        if (_policies == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(_condition.GetXAttribute("condition"));
        children.AddRange(_policies.ToArray());

        return new XElement("when", children.ToArray());
    }
}