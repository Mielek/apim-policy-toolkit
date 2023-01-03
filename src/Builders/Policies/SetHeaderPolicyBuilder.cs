namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class SetHeaderPolicyBuilder
    {
        IExpression<string>? _name;
        ImmutableList<IExpression<string>>.Builder? _values;
        IExpression<string>? _existAction;

        public SetHeaderPolicyBuilder ExistAction(ExistAction existAction)
        {
            return ExistAction(Translate(existAction));
        }

        private string Translate(Model.Policies.ExistAction existAction) => existAction switch
        {
            Model.Policies.ExistAction.Override => "override",
            Model.Policies.ExistAction.Append => "append",
            Model.Policies.ExistAction.Delete => "delete",
            Model.Policies.ExistAction.Skip => "skip",
            _ => throw new Exception(),
        };

        public SetHeaderPolicy Build()
        {
            if (_name == null) throw new NullReferenceException();

            return new SetHeaderPolicy(_name, _values?.ToImmutable(), _existAction);
        }

    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SetHeader(Action<SetHeaderPolicyBuilder> configurator)
        {
            var builder = new SetHeaderPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}