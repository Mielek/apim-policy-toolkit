using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class RateLimitByKeyPolicyHandler : MarshallerHandler<RateLimitByKeyPolicy>
{
    public override void Marshal(Marshaller marshaller, RateLimitByKeyPolicy element)
    {
        marshaller.Writer.WriteStartElement("rate-limit-by-key");
        marshaller.Writer.WriteAttribute("calls", element.Calls);
        marshaller.Writer.WriteAttribute("renewal-period", element.RenewalPeriod);
        marshaller.Writer.WriteExpressionAsAttribute("counter-key", element.CounterKey);

        marshaller.Writer.WriteNullableExpressionAsAttribute("increment-condition", element.IncrementCondition);
        marshaller.Writer.WriteNullableAttribute("increment-count", element.IncrementCount);
        marshaller.Writer.WriteNullableAttribute("retry-after-header-name", element.RetryAfterHeaderName);
        marshaller.Writer.WriteNullableAttribute("retry-after-variable-name", element.RetryAfterVariableName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-header-name", element.RemainingCallsHeaderName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-variable-name", element.RemainingCallsVariableName);
        marshaller.Writer.WriteNullableAttribute("total-calls-header-name", element.TotalCallsHeaderName);

        marshaller.Writer.WriteEndElement();
    }
}