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
public partial class QuotaByKeyPolicyBuilder : BaseBuilder<QuotaByKeyPolicyBuilder>
{
    private IExpression<string>? _counterKey;
    private uint? _renewalPeriod;
    private uint? _calls;
    private uint? _bandwidth;
    private IExpression<bool>? _incrementCondition;
    private DateTime? _firstPeriodStart;

    public XElement Build()
    {
        if (_counterKey == null) throw new PolicyValidationException("CounterKey is required for QuotaByKey");
        if (_renewalPeriod == null) throw new PolicyValidationException("RenewalPeriod is required for QuotaByKey");

        var element = CreateElement("quota-by-key");
        element.Add(_counterKey.GetXAttribute("counter-key"));
        element.Add(new XAttribute("renewal-period", _renewalPeriod));

        if (_calls != null)
        {
            element.Add(new XAttribute("calls", _calls));
        }
        if (_bandwidth != null)
        {
            element.Add(new XAttribute("bandwidth", _bandwidth));
        }
        if (_incrementCondition != null)
        {
            element.Add(_incrementCondition.GetXAttribute("increment-condition"));
        }
        if (_firstPeriodStart != null)
        {
            element.Add(new XAttribute("first-period-start", _firstPeriodStart));
        }

        return element;
    }
}