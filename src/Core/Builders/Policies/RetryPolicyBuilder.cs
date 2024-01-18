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
public partial class RetryPolicyBuilder<TSectionBuilder> : BaseBuilder<RetryPolicyBuilder<TSectionBuilder>> where TSectionBuilder : PolicySectionBuilder, new()
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
        if (_condition == null) throw new PolicyValidationException("Condition is required for Retry");
        if (_count == null) throw new PolicyValidationException("Count is required for Retry");
        if (_interval == null) throw new PolicyValidationException("Interval is required for Retry");
        if (_policies == null) throw new PolicyValidationException("Policies is required for Retry");

        var element = this.CreateElement("retry");
        element.Add(_condition.GetXAttribute("condition"));
        element.Add(new XAttribute("count", _count));
        element.Add(new XAttribute("interval", _interval));

        if (_maxInterval != null)
        {
            element.Add(new XAttribute("max-interval", _maxInterval));
        }
        if (_delta != null)
        {
            element.Add(new XAttribute("delta", _delta));
        }
        if (_firstFastRetry != null)
        {
            element.Add(_firstFastRetry.GetXAttribute("first-fast-retry"));
        }

        element.Add(_policies);

        return element;
    }
}
