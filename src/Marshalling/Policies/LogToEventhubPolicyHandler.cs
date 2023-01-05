
using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class LogToEventhubPolicyHandler : MarshallerHandler<LogToEventhubPolicy>
{
    public override void Marshal(Marshaller marshaller, LogToEventhubPolicy element)
    {
        marshaller.Writer.WriteStartElement("log-to-eventhub");

        marshaller.Writer.WriteAttribute("logger-id", element.LoggerId);
        marshaller.Writer.WriteNullableAttribute("partition-id", element.PartitionId);
        marshaller.Writer.WriteNullableAttribute("partition-key", element.PartitionKey);
        marshaller.Writer.WriteExpression(element.Value);

        marshaller.Writer.WriteEndElement();
    }
}