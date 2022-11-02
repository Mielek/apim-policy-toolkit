namespace Mielek.Builders.Policies
{
    using Mielek.Builders.Expressions;

    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    public class SetStatusPolicyBuilder
    {
        IExpression? _code;
        IExpression? _reason;

        public SetStatusPolicyBuilder Code(Action<ExpressionBuilder> configurator)
        {
            _code = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public SetStatusPolicyBuilder Code(ushort code)
        {
            return Code(config => config.Constant($"{code}"));
        }

        public SetStatusPolicyBuilder Reason(Action<ExpressionBuilder> configurator)
        {
            _reason = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public SetStatusPolicyBuilder Reason(string reason)
        {
            return Reason(config => config.Constant(reason));
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
            this._sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}