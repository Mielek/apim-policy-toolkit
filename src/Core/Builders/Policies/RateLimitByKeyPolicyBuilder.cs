namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class RateLimitByKeyPolicyBuilder
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
        if (_calls == null) throw new NullReferenceException();
        if (_renewalPeriod == null) throw new NullReferenceException();
        if (_counterKey == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(new XAttribute("calls", _calls));
        children.Add(new XAttribute("renewal-period", _renewalPeriod));
        children.Add(_counterKey.GetXAttribute("counter-key"));

        if (_incrementCondition != null)
        {
            children.Add(_incrementCondition.GetXAttribute("increment-condition"));
        }
        if (_incrementCount != null)
        {
            children.Add(new XAttribute("increment-count", _incrementCount));
        }
        if (_retryAfterHeaderName != null)
        {
            children.Add(new XAttribute("retry-after-header-name", _retryAfterHeaderName));
        }
        if (_retryAfterVariableName != null)
        {
            children.Add(new XAttribute("retry-after-variable-name", _retryAfterVariableName));
        }
        if (_remainingCallsHeaderName != null)
        {
            children.Add(new XAttribute("remaining-calls-header-name", _remainingCallsHeaderName));
        }
        if (_remainingCallsVariableName != null)
        {
            children.Add(new XAttribute("remaining-calls-variable-name", _remainingCallsVariableName));
        }
        if (_totalCallsHeaderName != null)
        {
            children.Add(new XAttribute("total-calls-header-name", _totalCallsHeaderName));
        }

        return new XElement("rate-limit-by-key", children.ToArray());
    }
}