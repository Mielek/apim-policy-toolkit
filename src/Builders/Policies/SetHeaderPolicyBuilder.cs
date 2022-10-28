namespace Mielek. Builders.Policies
{
    using System.Collections.Immutable;
    using Mielek.Builders.Expressions;
    using Mielek.Model.Policies;
    using Mielek.Model.Expressions;

    public class SetHeaderPolicyBuilder
    {
        IExpression? _name;
        ImmutableList<IExpression>.Builder? _values;
        IExpression? _existAction;

        public SetHeaderPolicyBuilder Name(string name)
        {
            _name = new ConstantExpression(name);
            return this;
        }

        public SetHeaderPolicyBuilder Name(Action<ExpressionBuilder> configurator)
        {
            _name = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public SetHeaderPolicyBuilder Value(string value)
        {
            return Value(config => config.Constant(value));
        }

        public SetHeaderPolicyBuilder Value(Action<ExpressionBuilder> configurator)
        {
            var value = ExpressionBuilder.BuildFromConfiguration(configurator);
            (_values ??= ImmutableList.CreateBuilder<IExpression>()).Add(value);
            return this;
        }

        public SetHeaderPolicyBuilder ExistAction(Action<ExpressionBuilder> configurator)
        {
            _existAction = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public SetHeaderPolicyBuilder ExistAction(Model.Policies.ExistAction existAction)
        {
            return ExistAction(config => config.Constant(Translate(existAction)));
        }

        private string Translate(Model.Policies.ExistAction existAction) => existAction switch
        {
            Model.Policies.ExistAction.Override => "override",
            Model.Policies.ExistAction.Append => "append",
            Model.Policies.ExistAction.Delete => "delete",
            Model.Policies.ExistAction.Skip => "skip",
            _ => throw new Exception(),
        };

        internal SetHeaderPolicy Build()
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
            this.sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
