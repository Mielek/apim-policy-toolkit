using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class QuotaByKeyPolicyHandler : MarshallerHandler<QuotaByKeyPolicy>
{
    public override void Marshal(Marshaller marshaller, QuotaByKeyPolicy element)
    {
        marshaller.Writer.WriteStartElement("quota-by-key");
        marshaller.Writer.WriteExpressionAsAttribute("counter-key", element.CounterKey);
        marshaller.Writer.WriteAttribute("renewal-period", $"{element.RenewalPeriod}");

        if (element.Calls != null) marshaller.Writer.WriteAttribute("calls", $"{element.Calls}");
        if (element.Bandwidth != null) marshaller.Writer.WriteAttribute("bandwidth", $"{element.Bandwidth}");

        if (element.IncrementCondition != null)
        {
            marshaller.Writer.WriteExpressionAsAttribute("increment-condition", element.IncrementCondition);
        }

        marshaller.Writer.WriteEndElement();
    }
}