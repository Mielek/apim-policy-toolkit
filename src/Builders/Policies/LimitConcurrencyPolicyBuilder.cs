namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class LimitConcurrencyPolicyBuilder
    {
        private IExpression<string>? _key;
        private uint? _maxCount;

        [IgnoreBuilderField]
        private ICollection<XElement>? _policies;

        public LimitConcurrencyPolicyBuilder Policies(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
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

