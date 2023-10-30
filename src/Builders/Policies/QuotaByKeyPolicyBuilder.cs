namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class QuotaByKeyPolicyBuilder
    {
        private IExpression<string>? _counterKey;
        private uint? _renewalPeriod;
        private uint? _calls;
        private uint? _bandwidth;
        private IExpression<bool>? _incrementCondition;
        private DateTime? _firstPeriodStart;

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