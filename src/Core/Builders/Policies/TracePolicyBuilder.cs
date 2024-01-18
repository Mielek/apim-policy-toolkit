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
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class TracePolicyBuilder : BaseBuilder<TracePolicyBuilder>
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
        if (_source == null) throw new PolicyValidationException("Source is required for Trace");
        if (_message == null) throw new PolicyValidationException("Message is required for Trace");

        var element = this.CreateElement("trace");

        element.Add(new XAttribute("source", _source));
        if (_severity != null)
        {
            element.Add(new XAttribute("severity", TranslateSeverity(_severity)));
        }
        element.Add(new XElement("message", _message.GetXText()));

        if (_metadatas != null && _metadatas.Count > 0)
        {
            element.Add(_metadatas.ToArray());
        }

        return element;
    }

    private static string TranslateSeverity(TraceSeverity? severity) => severity switch
    {
        TraceSeverity.Verbose => "verbose",
        TraceSeverity.Information => "information",
        TraceSeverity.Error => "error",
        _ => throw new PolicyValidationException("Unknown severity for Trace"),
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