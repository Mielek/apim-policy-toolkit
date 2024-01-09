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