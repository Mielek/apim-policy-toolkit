namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class SetStatusPolicyBuilder
    {
        private IExpression<string>? _code;
        private IExpression<string>? _reason;

        public SetStatusPolicyBuilder Code(ushort code)
        {
            return Code($"{code}");
        }

        public SetStatusPolicy Build()
        {
            if (_code == null) throw new NullReferenceException();

            return new SetStatusPolicy(_code, _reason);
        }

    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;
    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SetStatus(Action<SetStatusPolicyBuilder> configurator)
        {
            var builder = new SetStatusPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}