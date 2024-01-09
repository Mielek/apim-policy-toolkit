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
public partial class QuotaByKeyPolicyBuilder
{
    private IExpression<string>? _counterKey;
    private uint? _renewalPeriod;
    private uint? _calls;
    private uint? _bandwidth;
    private IExpression<bool>? _incrementCondition;
    private DateTime? _firstPeriodStart;

    public XElement Build()
    {
        if (_counterKey == null) throw new NullReferenceException();
        if (_renewalPeriod == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(_counterKey.GetXAttribute("counter-key"));
        children.Add(new XAttribute("renewal-period", _renewalPeriod));

        if (_calls != null)
        {
            children.Add(new XAttribute("calls", _calls));
        }
        if (_bandwidth != null)
        {
            children.Add(new XAttribute("bandwidth", _bandwidth));
        }
        if (_incrementCondition != null)
        {
            children.Add(_incrementCondition.GetXAttribute("increment-condition"));
        }
        if (_firstPeriodStart != null)
        {
            children.Add(new XAttribute("first-period-start", _firstPeriodStart));
        }

        return new XElement("quota-by-key", children.ToArray());
    }
}