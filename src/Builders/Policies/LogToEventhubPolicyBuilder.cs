namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class LogToEventhubPolicyBuilder
    {
        private string? _loggerId;
        private IExpression<string>? _value;
        private string? _partitionId;
        private string? _partitionKey;

        public LogToEventhubPolicy Build()
        {
            if (_loggerId == null) throw new NullReferenceException();
            if (_value == null) throw new NullReferenceException();
            
            return new LogToEventhubPolicy(_loggerId, _value, _partitionId, _partitionKey);
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder LimitConcurrency(Action<LogToEventhubPolicyBuilder> configurator)
        {
            var builder = new LogToEventhubPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

