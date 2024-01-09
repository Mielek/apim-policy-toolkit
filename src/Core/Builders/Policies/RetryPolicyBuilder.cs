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
public partial class RetryPolicyBuilder<TSectionBuilder> where TSectionBuilder : PolicySectionBuilder, new()
{
    private IExpression<bool>? _condition;
    private uint? _count;
    private uint? _interval;
    [IgnoreBuilderField]
    private ICollection<XElement>? _policies;
    private uint? _maxInterval;
    private uint? _delta;
    private IExpression<string>? _firstFastRetry;

    public RetryPolicyBuilder<TSectionBuilder> Policies(Action<TSectionBuilder> configurator)
    {
        var builder = new TSectionBuilder();
        configurator(builder);
        _policies = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_condition == null) throw new NullReferenceException();
        if (_count == null) throw new NullReferenceException();
        if (_interval == null) throw new NullReferenceException();
        if (_policies == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(_condition.GetXAttribute("condition"));
        children.Add(new XAttribute("count", _count));
        children.Add(new XAttribute("interval", _interval));

        if (_maxInterval != null)
        {
            children.Add(new XAttribute("max-interval", _maxInterval));
        }
        if (_delta != null)
        {
            children.Add(new XAttribute("delta", _delta));
        }
        if (_firstFastRetry != null)
        {
            children.Add(_firstFastRetry.GetXAttribute("first-fast-retry"));
        }

        children.AddRange(_policies);

        return new XElement("retry", children.ToArray());
    }
}
