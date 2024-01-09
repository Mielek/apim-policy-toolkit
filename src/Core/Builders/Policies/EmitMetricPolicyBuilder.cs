namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
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
            children.Add(_value.GetXAttribute("value"));
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