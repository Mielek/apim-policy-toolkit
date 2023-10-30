namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
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

        public RateLimitByKeyPolicy Build()
        {
            if (_calls == null) throw new NullReferenceException();
            if (_renewalPeriod == null) throw new NullReferenceException();
            if (_counterKey == null) throw new NullReferenceException();

            return new RateLimitByKeyPolicy(
                _calls.Value, 
                _renewalPeriod.Value,
                _counterKey,
                _incrementCondition,
                _incrementCount,
                _retryAfterHeaderName,
                _retryAfterVariableName, 
                _remainingCallsHeaderName,
                _remainingCallsVariableName,
                _totalCallsHeaderName);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder RateLimitByKey(Action<RateLimitByKeyPolicyBuilder> configurator)
        {
            var builder = new RateLimitByKeyPolicyBuilder();
            configurator(builder);
            this._sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}