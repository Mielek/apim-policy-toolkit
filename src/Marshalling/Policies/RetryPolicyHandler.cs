using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class RetryPolicyHandler : MarshallerHandler<RetryPolicy>
{
    public override void Marshal(Marshaller marshaller, RetryPolicy element)
    {
        marshaller.Writer.WriteStartElement("retry");

        marshaller.Writer.WriteAttribute("condition", element.Condition);
        marshaller.Writer.WriteAttribute("count", element.Count);
        marshaller.Writer.WriteAttribute("interval", element.Interval);
        marshaller.Writer.WriteNullableAttribute("max-interval", element.MaxInterval);
        marshaller.Writer.WriteNullableAttribute("delta", element.Delta);
        marshaller.Writer.WriteNullableAttribute("first-fast-retry", element.FirstFastRetry);

        foreach (var policy in element.Policies)
        {
            policy.Accept(marshaller);
        }

        marshaller.Writer.WriteEndElement();
    }
}