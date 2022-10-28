namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Builders.Expressions;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;
    public class CheckHeaderPolicyBuilder
    {
        IExpression? _name;
        IExpression? _failedCheckCode;
        IExpression? _failedCheckErrorMessage;
        IExpression? _ignoreCase;
        ImmutableList<IExpression>.Builder? _values;

        public CheckHeaderPolicyBuilder Name(string name)
        {
            return Name(config => config.Constant(name));
        }
        public CheckHeaderPolicyBuilder Name(Action<ExpressionBuilder> configurator) {
            _name = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public CheckHeaderPolicyBuilder FailedCheckCode(ushort code)
        {
            return FailedCheckCode(config => config.Constant($"{code}"));
        }
        
        public CheckHeaderPolicyBuilder FailedCheckCode(Action<ExpressionBuilder> configurator) {
            _failedCheckCode = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }
        
        public CheckHeaderPolicyBuilder FailedCheckErrorMessage(string message)
        {
            return FailedCheckErrorMessage(config => config.Constant(message));
        }
        
        public CheckHeaderPolicyBuilder FailedCheckErrorMessage(Action<ExpressionBuilder> configurator) {
            _failedCheckErrorMessage = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public CheckHeaderPolicyBuilder IgnoreCase(bool ignoreCase)
        {
            return IgnoreCase(config => config.Constant($"{ignoreCase}"));
        }
        
        public CheckHeaderPolicyBuilder IgnoreCase(Action<ExpressionBuilder> configurator) {
            _ignoreCase = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public CheckHeaderPolicyBuilder Value(string value)
        {
            return Value(config => config.Constant(value));
        }
        
        public CheckHeaderPolicyBuilder Value(Action<ExpressionBuilder> configurator) {
            var value = ExpressionBuilder.BuildFromConfiguration(configurator);
            (_values ??= ImmutableList.CreateBuilder<IExpression>()).Add(value);
            return this;
        }

        internal CheckHeaderPolicy Build()
        {
            if(_name == null) throw new NullReferenceException();
            if(_failedCheckCode == null) throw new NullReferenceException();
            if(_failedCheckErrorMessage == null) throw new NullReferenceException();
            if(_ignoreCase == null) throw new NullReferenceException();
            if(_values == null) throw new NullReferenceException();

            return new CheckHeaderPolicy(_name, _failedCheckCode, _failedCheckErrorMessage, _ignoreCase, _values.ToImmutable());
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder CheckHeader(Action<CheckHeaderPolicyBuilder> configurator)
        {
            var builder = new CheckHeaderPolicyBuilder();
            configurator(builder);
            this.sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
