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
public partial class LogToEventhubPolicyBuilder : BaseBuilder<LogToEventhubPolicyBuilder>
{
    private string? _loggerId;
    private IExpression<string>? _value;
    private string? _partitionId;
    private string? _partitionKey;

    public XElement Build()
    {
        if (_loggerId == null) throw new PolicyValidationException("LoggerId is required for LogToEventhub");
        if (_value == null) throw new PolicyValidationException("Value is required for LogToEventhub");

        var element = CreateElement("log-to-eventhub");
        element.Add(new XAttribute("logger-id", _loggerId));

        if (_partitionId != null)
        {
            element.Add(new XAttribute("partition-id", _partitionId));
        }

        if (_partitionKey != null)
        {
            element.Add(new XAttribute("partition-key", _partitionKey));
        }

        element.Add(_value.GetXText());

        return element;
    }
}