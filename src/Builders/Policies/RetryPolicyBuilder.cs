namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;


    [GenerateBuilderSetters]
    public partial class RetryPolicyBuilder
    {
        private IExpression<bool>? _condition;
        private uint? _count;
        private uint? _interval;
        [IgnoreBuilderField]
        private ICollection<IPolicy>? _policies;
        private uint? _maxInterval;
        private uint? _delta;
        private IExpression<string>? _firstFastRetry;

        public RetryPolicyBuilder Policies(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
            configurator(builder);
            _policies = builder.Build();
            return this;
        }

        public RetryPolicy Build()
        {
            if(_condition == null) throw new NullReferenceException();
            if(_count == null) throw new NullReferenceException();
            if(_interval == null) throw new NullReferenceException();
            if(_policies == null) throw new NullReferenceException();

            return new RetryPolicy(_condition, _count.Value, _interval.Value, _policies, _maxInterval, _delta, _firstFastRetry);
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder Retry(Action<RetryPolicyBuilder> configurator)
        {
            var builder = new RetryPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
