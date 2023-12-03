namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

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

        protected virtual object[] BuildBasic()
        {
            if (_calls == null) throw new NullReferenceException();
            if (_renewalPeriod == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            children.Add(new XAttribute("calls", _calls));
            children.Add(new XAttribute("renewal-period", _renewalPeriod));

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

            return children.ToArray();
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

        protected override object[] BuildBasic()
        {
            if (_name == null && _id == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();

            if (_name != null)
            {
                children.Add(new XAttribute("name", _name));
            }
            if (_id != null)
            {
                children.Add(new XAttribute("id", _id));
            }

            children.AddRange(base.BuildBasic());

            return children.ToArray();
        }
    }

    public class RateLimitApiOperationBuilder : RateLimitWithIdentificationBuilder<RateLimitApiOperationBuilder>
    {
        public XElement Build()
        {
            return new XElement("operation", BuildBasic());
        }
    }

    public class RateLimitApiBuilder : RateLimitWithIdentificationBuilder<RateLimitApiBuilder>
    {
        private List<XElement>? _operations = null;

        public RateLimitApiBuilder Operation(Action<RateLimitApiOperationBuilder> config)
        {
            var builder = new RateLimitApiOperationBuilder();
            config(builder);
            (_operations ??= new List<XElement>()).Add(builder.Build());
            return this;
        }

        public XElement Build()
        {
            return new XElement("api", BuildBasic());
        }

    }

    public class RateLimitPolicyBuilder : RateLimitBaseBuilder<RateLimitPolicyBuilder>
    {
        private List<XElement>? _apis = null;

        public RateLimitPolicyBuilder Api(Action<RateLimitApiBuilder> config)
        {
            var builder = new RateLimitApiBuilder();
            config(builder);
            (_apis ??= new List<XElement>()).Add(builder.Build());
            return this;
        }

        public XElement Build()
        {
            return new XElement("rate-limit", BuildBasic());
        }

    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

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