using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class LimitConcurrencyPolicyHandler : MarshallerHandler<LimitConcurrencyPolicy>
{
    public override void Marshal(Marshaller marshaller, LimitConcurrencyPolicy element)
    {
        marshaller.Writer.WriteStartElement("limit-concurrency");
        marshaller.Writer.WriteExpressionAsAttribute("key", element.Key);
        marshaller.Writer.WriteAttribute("max-count", element.MaxCount);

        foreach (var policy in element.Policies)
        {
            policy.Accept(marshaller);
        }

        marshaller.Writer.WriteEndElement();
    }
}