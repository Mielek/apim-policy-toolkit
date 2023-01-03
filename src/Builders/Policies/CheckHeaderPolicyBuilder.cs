namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class CheckHeaderPolicyBuilder
    {
        IExpression<string>? _name;
        IExpression<string>? _failedCheckHttpCode;
        IExpression<string>? _failedCheckErrorMessage;
        IExpression<bool>? _ignoreCase;
        ImmutableList<IExpression<string>>.Builder? _values;

        public CheckHeaderPolicyBuilder FailedCheckHttpCode(ushort code)
        {
            return FailedCheckHttpCode($"{code}");
        }

        public CheckHeaderPolicy Build()
        {
            if (_name == null) throw new NullReferenceException();
            if (_failedCheckHttpCode == null) throw new NullReferenceException();
            if (_failedCheckErrorMessage == null) throw new NullReferenceException();
            if (_ignoreCase == null) throw new NullReferenceException();
            if (_values == null) throw new NullReferenceException();

            return new CheckHeaderPolicy(_name, _failedCheckHttpCode, _failedCheckErrorMessage, _ignoreCase, _values.ToImmutable());
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
            this._sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}