namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class LimitConcurrencyPolicyBuilder<TSectionBuilder> where TSectionBuilder : PolicySectionBuilder, new()
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
            if (_key == null) throw new NullReferenceException();
            if (_maxCount == null) throw new NullReferenceException();
            if (_policies == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            children.Add(_key.GetXAttribute("key"));
            children.Add(new XAttribute("max-count", _maxCount));
            children.AddRange(_policies.ToArray());

            return new XElement("limit-concurrency", children.ToArray());
        }
    }
}


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

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