namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generators.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class SetHeaderPolicyBuilder
    {
        private IExpression<string>? _name;
        private ImmutableList<IExpression<string>>.Builder? _values;
        private IExpression<string>? _existsAction;

        public SetHeaderPolicyBuilder ExistsAction(SetHeaderExistsAction existsAction)
        {
            return ExistsAction(Translate(existsAction));
        }

        private string Translate(SetHeaderExistsAction existsAction) => existsAction switch
        {
            SetHeaderExistsAction.Override => "override",
            SetHeaderExistsAction.Append => "append",
            SetHeaderExistsAction.Delete => "delete",
            SetHeaderExistsAction.Skip => "skip",
            _ => throw new Exception(),
        };

        public SetHeaderPolicy Build()
        {
            if (_name == null) throw new NullReferenceException();

            return new SetHeaderPolicy(_name, _values?.ToImmutable(), _existsAction);
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