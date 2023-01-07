using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class QuotaByKeyPolicyHandler : MarshallerHandler<QuotaByKeyPolicy>
{
    public override void Marshal(Marshaller marshaller, QuotaByKeyPolicy element)
    {
        marshaller.Writer.WriteStartElement("quota-by-key");
        marshaller.Writer.WriteAttribute("counter-key", element.CounterKey);
        marshaller.Writer.WriteAttribute("renewal-period", element.RenewalPeriod);

        marshaller.Writer.WriteNullableAttribute("calls", element.Calls);
        marshaller.Writer.WriteNullableAttribute("bandwidth", element.Bandwidth);

        marshaller.Writer.WriteNullableAttribute("increment-condition", element.IncrementCondition);

        marshaller.Writer.WriteEndElement();
    }
}