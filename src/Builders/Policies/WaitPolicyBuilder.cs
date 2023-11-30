namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class WaitPolicyBuilder
    {
        public enum WaitFor { All, Any }

        private WaitFor? _for;
        [IgnoreBuilderField]
        private ICollection<XElement>? _policies;

        public WaitPolicyBuilder Policies(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
            configurator(builder);
            _policies = builder.Build();
            return this;
        }

        public XElement Build()
        {
            if (_policies == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            if(_for != null)
            {
                children.Add(new XAttribute("for", TranslateFor(_for)));
            }

            children.AddRange(_policies.ToArray());

            return new XElement("wait", _for);
        }
        private static string TranslateFor(WaitFor? waitFor) => waitFor switch
        {
            WaitFor.All => "all",
            WaitFor.Any => "any",
            _ => throw new Exception(),
        };
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder Wait(Action<WaitPolicyBuilder> configurator)
        {
            var builder = new WaitPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
