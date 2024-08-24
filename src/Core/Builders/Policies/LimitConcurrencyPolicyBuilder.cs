using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    [GenerateBuilderSetters]
    public partial class LimitConcurrencyPolicyBuilder<TSectionBuilder> : BaseBuilder<LimitConcurrencyPolicyBuilder<TSectionBuilder>> where TSectionBuilder : PolicySectionBuilder, new()
    {
        private IExpression<string>? _key;
        private uint? _maxCount;

        [IgnoreBuilderField]
        private ICollection<XElement>? _policies;

        public LimitConcurrencyPolicyBuilder<TSectionBuilder> Policies(Action<TSectionBuilder> configurator)
        {
            var builder = new TSectionBuilder();
            configurator(builder);
            _policies = builder.Build();
            return this;
        }

        public XElement Build()
        {
            if (_key == null) throw new PolicyValidationException("Key is required for LimitConcurrency");
            if (_maxCount == null) throw new PolicyValidationException("MaxCount is required for LimitConcurrency");
            if (_policies == null) throw new PolicyValidationException("Policies is required for LimitConcurrency");

            var element = CreateElement("limit-concurrency");
            element.Add(_key.GetXAttribute("key"));
            element.Add(new XAttribute("max-count", _maxCount));
            element.Add(_policies.ToArray());

            return element;
        }
    }
}


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    public partial class InboundSectionBuilder
    {
        public InboundSectionBuilder LimitConcurrency(Action<LimitConcurrencyPolicyBuilder<InboundSectionBuilder>> configurator)
        {
            var builder = new LimitConcurrencyPolicyBuilder<InboundSectionBuilder>();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
    public partial class OutboundSectionBuilder
    {
        public OutboundSectionBuilder LimitConcurrency(Action<LimitConcurrencyPolicyBuilder<OutboundSectionBuilder>> configurator)
        {
            var builder = new LimitConcurrencyPolicyBuilder<OutboundSectionBuilder>();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
    public partial class BackendSectionBuilder
    {
        public BackendSectionBuilder LimitConcurrency(Action<LimitConcurrencyPolicyBuilder<BackendSectionBuilder>> configurator)
        {
            var builder = new LimitConcurrencyPolicyBuilder<BackendSectionBuilder>();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
    public partial class OnErrorSectionBuilder
    {
        public OnErrorSectionBuilder LimitConcurrency(Action<LimitConcurrencyPolicyBuilder<OnErrorSectionBuilder>> configurator)
        {
            var builder = new LimitConcurrencyPolicyBuilder<OnErrorSectionBuilder>();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
    public partial class PolicyFragmentBuilder
    {
        public PolicyFragmentBuilder LimitConcurrency(Action<LimitConcurrencyPolicyBuilder<PolicyFragmentBuilder>> configurator)
        {
            var builder = new LimitConcurrencyPolicyBuilder<PolicyFragmentBuilder>();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}