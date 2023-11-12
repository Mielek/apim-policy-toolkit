namespace Mielek.Builders.Policies
{
    using Mielek.Generators.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class SetVariablePolicyBuilder
    {
        private string? _name;
        private IExpression<string>? _value;

        public SetVariablePolicy Build()
        {
            if (_name == null) throw new NullReferenceException();
            if (_value == null) throw new NullReferenceException();

            return new SetVariablePolicy(_name, _value);
        }

    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;
    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SetVariable(Action<SetVariablePolicyBuilder> configurator)
        {
            var builder = new SetVariablePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}