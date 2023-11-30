namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class LogToEventhubPolicyBuilder
    {
        private string? _loggerId;
        private IExpression<string>? _value;
        private string? _partitionId;
        private string? _partitionKey;

        public XElement Build()
        {
            if (_loggerId == null) throw new NullReferenceException();
            if (_value == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            children.Add(new XAttribute("logger-id", _loggerId));

            if (_partitionId != null)
            {
                children.Add(new XAttribute("partition-id", _partitionId));
            }

            if (_partitionKey != null)
            {
                children.Add(new XAttribute("partition-key", _partitionKey));
            }

            children.Add(_value.GetXText());

            return new XElement("log-to-eventhub", children.ToArray());
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

