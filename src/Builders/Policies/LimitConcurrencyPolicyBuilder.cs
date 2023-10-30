namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class LimitConcurrencyPolicyBuilder
    {
        private IExpression<string>? _key;
        private uint? _maxCount;

        [IgnoreBuilderField]
        private ICollection<IPolicy>? _policies;

        public LimitConcurrencyPolicyBuilder Policies(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
            configurator(builder);
            _policies = builder.Build();
            return this;
        }

        public LimitConcurrencyPolicy Build()
        {
            if(_key == null) throw new NullReferenceException();
            if(_maxCount == null) throw new NullReferenceException();
            if(_policies == null) throw new NullReferenceException();

            return new LimitConcurrencyPolicy(_key, _maxCount.Value, _policies);
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder LimitConcurrency(Action<LimitConcurrencyPolicyBuilder> configurator)
        {
            var builder = new LimitConcurrencyPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

