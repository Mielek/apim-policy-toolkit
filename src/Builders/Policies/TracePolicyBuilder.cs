namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class TracePolicyBuilder
    {
        public enum TraceSeverity { Verbose, Information, Error }

        private string? _source;
        private IExpression<string>? _message;
        private TraceSeverity? _severity;

        [IgnoreBuilderField]
        private ImmutableList<XElement>.Builder? _metadatas;

        public TracePolicyBuilder Metadata(Action<TraceMetadataBuilder> configurator)
        {
            var builder = new TraceMetadataBuilder();
            configurator(builder);
            (_metadatas ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
            return this;
        }

        public XElement Build()
        {
            if (_source == null) throw new NullReferenceException();
            if (_message == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(new XAttribute("source", _source));
            if (_severity != null)
            {
                children.Add(new XAttribute("severity", TranslateSeverity(_severity)));
            }
            children.Add(new XElement("message", _message.GetXText()));

            if (_metadatas != null && _metadatas.Count > 0)
            {
                children.AddRange(_metadatas.ToArray());
            }

            return new XElement("trace", children.ToArray());
        }

        private static string TranslateSeverity(TraceSeverity? severity) => severity switch
        {
            TraceSeverity.Verbose => "verbose",
            TraceSeverity.Information => "information",
            TraceSeverity.Error => "error",
            _ => throw new Exception(),
        };
    }


    [GenerateBuilderSetters]
    public partial class TraceMetadataBuilder
    {
        private string? _name;
        private IExpression<string>? _value;

        public XElement Build()
        {
            if (_name == null) throw new NullReferenceException();
            if (_value == null) throw new NullReferenceException();

            var children = new[]
            {
                new XAttribute("name", _name),
                _value.GetXAttribute("value")
            };
            return new XElement("metadata", children);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder Trace(Action<TracePolicyBuilder> configurator)
        {
            var builder = new TracePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}