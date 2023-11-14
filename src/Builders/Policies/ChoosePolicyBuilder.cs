namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generators.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    public partial class ChoosePolicyBuilder
    {
        private readonly ImmutableList<ChoosePolicyWhen>.Builder _whens = ImmutableList.CreateBuilder<ChoosePolicyWhen>();
        private ICollection<IPolicy>? _otherwise;

        public ChoosePolicyBuilder When(Action<ChooseWhenBuilder> configurator)
        {
            var builder = new ChooseWhenBuilder();
            configurator(builder);
            _whens.Add(builder.Build());
            return this;
        }

        public ChoosePolicyBuilder Otherwise(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
            configurator(builder);
            _otherwise = builder.Build();
            return this;
        }

        public ChoosePolicy Build()
        {
            return new ChoosePolicy(_whens.ToArray(), _otherwise?.ToArray());
        }
    }

    [GenerateBuilderSetters]
    public partial class ChooseWhenBuilder
    {
        private IExpression<bool>? _condition;

        [IgnoreBuilderField]
        private ICollection<IPolicy>? _policies;

        public ChooseWhenBuilder Policies(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
            configurator(builder);
            _policies = builder.Build();
            return this;
        }

        public ChoosePolicyWhen Build()
        {
            if(_condition == null) throw new NullReferenceException();
            if(_policies == null) throw new NullReferenceException();

            return new ChoosePolicyWhen(_condition, _policies.ToArray());
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder Choose(Action<ChoosePolicyBuilder> configurator)
        {
            var builder = new ChoosePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

