namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class EmitMetricPolicyBuilder
    {
        private string? _name;
        [IgnoreBuilderField]
        private readonly ImmutableList<XElement>.Builder _dimensions = ImmutableList.CreateBuilder<XElement>();
        private IExpression<string>? _value;
        private string? _namespace;

        public EmitMetricPolicyBuilder Dimension(Action<EmitMetricDimensionBuilder> configuration)
        {
            var builder = new EmitMetricDimensionBuilder();
            configuration(builder);
            _dimensions.Add(builder.Build());
            return this;
        }

        public XElement Build()
        {
            if (_name == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(new XAttribute("name", _name));

            if (_value != null)
            {
                children.Add(new XAttribute("value", _value.GetXText()));
            }

            if (_namespace != null)
            {
                children.Add(new XAttribute("namespace", _namespace));
            }

            children.Add(_dimensions);

            return new XElement("emit-metric", children.ToArray());
        }
    }

    [GenerateBuilderSetters]
    public partial class EmitMetricDimensionBuilder
    {
        private string? _name;
        private IExpression<string>? _value;

        public XElement Build()
        {
            if (_name == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            children.Add(new XAttribute("name", _name));

            if (_value != null)
            {
                children.Add(new XAttribute("value", _value));
            }

            return new XElement("dimension", children.ToArray());
        }
    }

}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder EmitMetric(Action<EmitMetricPolicyBuilder> configurator)
        {
            var builder = new EmitMetricPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

