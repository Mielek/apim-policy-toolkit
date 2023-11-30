namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
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
            children.Add(new XAttribute("counter-key", _counterKey.GetXText()));
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
                children.Add(new XAttribute("increment-condition", _incrementCondition.GetXText()));
            }
            if (_firstPeriodStart != null)
            {
                children.Add(new XAttribute("first-period-start", _firstPeriodStart));
            }

            return new XElement("quota-by-key", children.ToArray());
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder QuotaByKey(Action<QuotaByKeyPolicyBuilder> configurator)
        {
            var builder = new QuotaByKeyPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}