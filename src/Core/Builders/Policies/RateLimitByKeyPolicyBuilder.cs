using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class RateLimitByKeyPolicyBuilder : BaseBuilder<RateLimitByKeyPolicyBuilder>
{
    private uint? _calls;
    private uint? _renewalPeriod;
    private IExpression<string>? _counterKey;
    private IExpression<bool>? _incrementCondition;
    private uint? _incrementCount;
    private string? _retryAfterHeaderName;
    private string? _retryAfterVariableName;
    private string? _remainingCallsHeaderName;
    private string? _remainingCallsVariableName;
    private string? _totalCallsHeaderName;

    public XElement Build()
    {
        if (_calls == null) throw new PolicyValidationException("Calls is required for RateLimitByKey");
        if (_renewalPeriod == null) throw new PolicyValidationException("RenewalPeriod is required for RateLimitByKey");
        if (_counterKey == null) throw new PolicyValidationException("CounterKey is required for RateLimitByKey");

        var element = this.CreateElement("rate-limit-by-key");
        element.Add(new XAttribute("calls", _calls));
        element.Add(new XAttribute("renewal-period", _renewalPeriod));
        element.Add(_counterKey.GetXAttribute("counter-key"));

        if (_incrementCondition != null)
        {
            element.Add(_incrementCondition.GetXAttribute("increment-condition"));
        }
        if (_incrementCount != null)
        {
            element.Add(new XAttribute("increment-count", _incrementCount));
        }
        if (_retryAfterHeaderName != null)
        {
            element.Add(new XAttribute("retry-after-header-name", _retryAfterHeaderName));
        }
        if (_retryAfterVariableName != null)
        {
            element.Add(new XAttribute("retry-after-variable-name", _retryAfterVariableName));
        }
        if (_remainingCallsHeaderName != null)
        {
            element.Add(new XAttribute("remaining-calls-header-name", _remainingCallsHeaderName));
        }
        if (_remainingCallsVariableName != null)
        {
            element.Add(new XAttribute("remaining-calls-variable-name", _remainingCallsVariableName));
        }
        if (_totalCallsHeaderName != null)
        {
            element.Add(new XAttribute("total-calls-header-name", _totalCallsHeaderName));
        }

        return element;
    }
}