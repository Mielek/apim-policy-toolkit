namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class QuotaByKeyPolicyBuilder
    {
        IExpression? _counterKey;
        uint? _renewalPeriod;
        uint? _calls;
        uint? _bandwidth;
        IExpression? _incrementCondition;
        DateTime? _firstPeriodStart;

        public QuotaByKeyPolicy Build()
        {
            if (_counterKey == null) throw new NullReferenceException();
            if (_renewalPeriod == null) throw new NullReferenceException();

            return new QuotaByKeyPolicy(
                _counterKey,
                _renewalPeriod.Value,
                _calls,
                _bandwidth,
                _incrementCondition,
                _firstPeriodStart);
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