namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Policies;


    [GenerateBuilderSetters]
    public partial class WaitPolicyBuilder
    {
        private WaitFor? _for;
        [IgnoreBuilderField]
        private ICollection<IPolicy>? _policies;

        public WaitPolicyBuilder Policies(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
            configurator(builder);
            _policies = builder.Build();
            return this;
        }

        public WaitPolicy Build()
        {
            if(_policies == null) throw new NullReferenceException();

            return new WaitPolicy(_policies, _for);
        }
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
