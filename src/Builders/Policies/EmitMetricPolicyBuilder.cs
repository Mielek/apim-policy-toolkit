namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;


    [GenerateBuilderSetters]
    public partial class EmitMetricPolicyBuilder
    {
        string? _name;
        [IgnoreBuilderField]
        readonly ImmutableList<EmitMetricDimension>.Builder _dimensions = ImmutableList.CreateBuilder<EmitMetricDimension>();
        IExpression<string>? _value;
        string? _namespace;

        public EmitMetricPolicyBuilder Dimension(Action<EmitMetricDimensionBuilder> configuration)
        {
            var builder = new EmitMetricDimensionBuilder();
            configuration(builder);
            _dimensions.Add(builder.Build());
            return this;
        }

        public EmitMetricPolicy Build()
        {
            if (_name == null) throw new NullReferenceException();

            return new EmitMetricPolicy(_name, _dimensions.ToImmutable(), _value, _namespace);
        }
    }

    [GenerateBuilderSetters]
    public partial class EmitMetricDimensionBuilder
    {
        string? _name;
        IExpression<string>? _value;

        public EmitMetricDimension Build()
        {
            if (_name == null) throw new NullReferenceException();

            return new EmitMetricDimension(_name, _value);
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

