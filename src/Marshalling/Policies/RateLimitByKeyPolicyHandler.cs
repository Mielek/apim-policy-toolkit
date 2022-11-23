using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class RateLimitByKeyPolicyHandler : MarshallerHandler<RateLimitByKeyPolicy>
{
    public override void Marshal(Marshaller marshaller, RateLimitByKeyPolicy element)
    {
        marshaller.Writer.WriteStartElement("rate-limit-by-key");
        marshaller.Writer.WriteAttribute("calls", $"{element.Calls}");
        marshaller.Writer.WriteAttribute("renewal-period", $"{element.RenewalPeriod}");
        marshaller.Writer.WriteExpressionAsAttribute("counter-key", element.CounterKey);

        if (element.IncrementCondition != null)
        {
            marshaller.Writer.WriteExpressionAsAttribute("increment-condition", element.IncrementCondition);
        }
        if (element.IncrementCount != null)
        {
            marshaller.Writer.WriteAttribute("increment-count", $"{element.IncrementCount}");
        }
        if (element.RetryAfterHeaderName != null)
        {
            marshaller.Writer.WriteAttribute("retry-after-header-name", $"{element.RetryAfterHeaderName}");
        }
        if (element.RetryAfterVariableName != null)
        {
            marshaller.Writer.WriteAttribute("retry-after-variable-name", $"{element.RetryAfterVariableName}");
        }
        if (element.RemainingCallsHeaderName != null)
        {
            marshaller.Writer.WriteAttribute("remaining-calls-header-name", $"{element.RemainingCallsHeaderName}");
        }
        if (element.RemainingCallsVariableName != null)
        {
            marshaller.Writer.WriteAttribute("remaining-calls-variable-name", $"{element.RemainingCallsVariableName}");
        }
        if (element.TotalCallsHeaderName != null)
        {
            marshaller.Writer.WriteAttribute("total-calls-header-name", $"{element.TotalCallsHeaderName}");
        }

        marshaller.Writer.WriteEndElement();
    }
}