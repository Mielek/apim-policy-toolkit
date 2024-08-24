using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class EmitMetricPolicyBuilder : BaseBuilder<EmitMetricPolicyBuilder>
{
    private string? _name;
    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder _dimensions = ImmutableList.CreateBuilder<XElement>();
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
        if (_name == null) throw new PolicyValidationException("Name is required for EmitMetric");

        var element = CreateElement("emit-metric");

        element.Add(new XAttribute("name", _name));

        if (_value != null)
        {
            element.Add(_value.GetXAttribute("value"));
        }

        if (_namespace != null)
        {
            element.Add(new XAttribute("namespace", _namespace));
        }

        element.Add(_dimensions);

        return element;
    }
}

[GenerateBuilderSetters]
public partial class EmitMetricDimensionBuilder
{
    private string? _name;
    private IExpression<string>? _value;

    public XElement Build()
    {
        if (_name == null) throw new PolicyValidationException("Name is required for EmitMetric Dimension");

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(new XAttribute("name", _name));

        if (_value != null)
        {
            children.Add(new XAttribute("value", _value));
        }

        return new XElement("dimension", children.ToArray());
    }
}