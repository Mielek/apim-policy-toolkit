namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    public partial class ChoosePolicyBuilder
    {
        private readonly ImmutableList<XElement>.Builder _whens = ImmutableList.CreateBuilder<XElement>();
        private ICollection<XElement>? _otherwise;

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

        public XElement Build()
        {
            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(_whens.ToArray());

            if (_otherwise != null && _otherwise.Count > 0)
            {
                children.Add(new XElement("otherwise", _otherwise.ToArray()));
            }

            return new XElement("choose", children.ToArray());
        }
    }

    [GenerateBuilderSetters]
    public partial class ChooseWhenBuilder
    {
        private IExpression<bool>? _condition;

        [IgnoreBuilderField]
        private ICollection<XElement>? _policies;

        public ChooseWhenBuilder Policies(Action<PolicySectionBuilder> configurator)
        {
            var builder = new PolicySectionBuilder();
            configurator(builder);
            _policies = builder.Build();
            return this;
        }

        public XElement Build()
        {
            if (_condition == null) throw new NullReferenceException();
            if (_policies == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            children.Add(_condition.GetXAttribute("condition"));
            children.AddRange(_policies.ToArray());

            return new XElement("when", children.ToArray());
        }
    }
}


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

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

