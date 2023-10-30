namespace Mielek.Builders.Policies
{
    using Mielek.Model.Policies;

    public abstract class RateLimitBaseBuilder<T> where T : RateLimitBaseBuilder<T>
    {
        protected uint? _calls = null;
        protected uint? _renewalPeriod = null;
        protected string? _retryAfterHeaderName = null;
        protected string? _retryAfterVariableName = null;
        protected string? _remainingCallsHeaderName = null;
        protected string? _remainingCallsVariableName = null;
        protected string? _totalCallsHeaderName = null;

        public T Calls(uint value)
        {
            _calls = value;
            return (T)this;
        }
        public T RenewalPeriod(uint value)
        {
            _renewalPeriod = value;
            return (T)this;
        }
        public T RetryAfterHeaderName(string value)
        {
            _retryAfterHeaderName = value;
            return (T)this;
        }
        public T RetryAfterVariableName(string value)
        {
            _retryAfterVariableName = value;
            return (T)this;
        }
        public T RemainingCallsHeaderName(string value)
        {
            _remainingCallsHeaderName = value;
            return (T)this;
        }
        public T RemainingCallsVariableName(string value)
        {
            _remainingCallsVariableName = value;
            return (T)this;
        }
        public T TotalCallsHeaderName(string value)
        {
            _totalCallsHeaderName = value;
            return (T)this;
        }

        protected virtual void CheckRequiredParams()
        {
            if (_calls == null) throw new NullReferenceException();
            if (_renewalPeriod == null) throw new NullReferenceException();
        }
    }

    public abstract class RateLimitWithIdentificationBuilder<T> : RateLimitBaseBuilder<T> where T : RateLimitWithIdentificationBuilder<T>
    {
        protected string? _id = null;
        protected string? _name = null;

        public T Id(string value)
        {
            _id = value;
            return (T)this;
        }

        public T Name(string value)
        {
            _name = value;
            return (T)this;
        }

        protected override void CheckRequiredParams()
        {
            if (_name == null && _id == null) throw new NullReferenceException();
            base.CheckRequiredParams();
        }
    }

    public class RateLimitApiOperationBuilder : RateLimitWithIdentificationBuilder<RateLimitApiOperationBuilder>
    {
        public RateLimitApiOperation Build()
        {
            CheckRequiredParams();

            return new RateLimitApiOperation(
                _calls!.Value,
                _renewalPeriod!.Value,
                _name,
                _id,
                _retryAfterHeaderName,
                _retryAfterVariableName,
                _remainingCallsHeaderName,
                _remainingCallsVariableName,
                _totalCallsHeaderName
            );
        }
    }

    public class RateLimitApiBuilder : RateLimitWithIdentificationBuilder<RateLimitApiBuilder>
    {
        private List<RateLimitApiOperation>? _operations = null;

        public RateLimitApiBuilder Operation(Action<RateLimitApiOperationBuilder> config)
        {
            var builder = new RateLimitApiOperationBuilder();
            config(builder);
            (_operations ??= new List<RateLimitApiOperation>()).Add(builder.Build());
            return this;
        }

        public RateLimitApi Build()
        {
            CheckRequiredParams();

            return new RateLimitApi(
                _calls!.Value,
                _renewalPeriod!.Value,
                _name,
                _id,
                _retryAfterHeaderName,
                _retryAfterVariableName,
                _remainingCallsHeaderName,
                _remainingCallsVariableName,
                _totalCallsHeaderName,
                _operations
            );
        }

    }

    public class RateLimitPolicyBuilder : RateLimitBaseBuilder<RateLimitPolicyBuilder>
    {
        private List<RateLimitApi>? _apis = null;

        public RateLimitPolicyBuilder Api(Action<RateLimitApiBuilder> config)
        {
            var builder = new RateLimitApiBuilder();
            config(builder);
            (_apis ??= new List<RateLimitApi>()).Add(builder.Build());
            return this;
        }

        public RateLimitPolicy Build()
        {
            CheckRequiredParams();

            return new RateLimitPolicy(
                _calls!.Value,
                _renewalPeriod!.Value,
                _retryAfterHeaderName,
                _retryAfterVariableName,
                _remainingCallsHeaderName,
                _remainingCallsVariableName,
                _totalCallsHeaderName,
                _apis
            );
        }

    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder RateLimit(Action<RateLimitPolicyBuilder> configurator)
        {
            var builder = new RateLimitPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}