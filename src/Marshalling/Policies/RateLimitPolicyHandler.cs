using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class RateLimitPolicyHandler : MarshallerHandler<RateLimitPolicy>
{
    public override void Marshal(Marshaller marshaller, RateLimitPolicy element)
    {
        marshaller.Writer.WriteStartElement("rate-limit");

        MarshalBasicParams(marshaller, element);

        if (element.Apis != null && element.Apis.Count > 0)
        {
            foreach (var api in element.Apis)
            {
                MarshalApi(marshaller, api);
            }
        }

        marshaller.Writer.WriteEndElement();
    }

    public void MarshalApi(Marshaller marshaller, RateLimitApi element)
    {
        marshaller.Writer.WriteStartElement("api");
        marshaller.Writer.WriteNullableAttribute("name", element.Name);
        marshaller.Writer.WriteNullableAttribute("id", element.Id);
        MarshalBasicParams(marshaller, element);
        if (element.Operations != null && element.Operations.Count > 0)
        {
            foreach (var operation in element.Operations)
            {
                MarshalOperation(marshaller, operation);
            }
        }
        marshaller.Writer.WriteEndElement();
    }

    private void MarshalOperation(Marshaller marshaller, RateLimitApiOperation element)
    {
        marshaller.Writer.WriteStartElement("operation");
        marshaller.Writer.WriteNullableAttribute("name", element.Name);
        marshaller.Writer.WriteNullableAttribute("id", element.Id);
        MarshalBasicParams(marshaller, element);
        marshaller.Writer.WriteEndElement();
    }

    public void MarshalBasicParams(Marshaller marshaller, RateLimitPolicy element)
    {
        marshaller.Writer.WriteAttribute("calls", element.Calls);
        marshaller.Writer.WriteAttribute("renewal-period", element.RenewalPeriod);
        marshaller.Writer.WriteNullableAttribute("retry-after-header-name", element.RetryAfterHeaderName);
        marshaller.Writer.WriteNullableAttribute("retry-after-variable-name", element.RetryAfterVariableName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-header-name", element.RemainingCallsHeaderName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-variable-name", element.RemainingCallsVariableName);
        marshaller.Writer.WriteNullableAttribute("total-calls-header-name", element.TotalCallsHeaderName);
    }
    public void MarshalBasicParams(Marshaller marshaller, RateLimitApi element)
    {
        marshaller.Writer.WriteAttribute("calls", element.Calls);
        marshaller.Writer.WriteAttribute("renewal-period", element.RenewalPeriod);
        marshaller.Writer.WriteNullableAttribute("retry-after-header-name", element.RetryAfterHeaderName);
        marshaller.Writer.WriteNullableAttribute("retry-after-variable-name", element.RetryAfterVariableName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-header-name", element.RemainingCallsHeaderName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-variable-name", element.RemainingCallsVariableName);
        marshaller.Writer.WriteNullableAttribute("total-calls-header-name", element.TotalCallsHeaderName);

    }
    public void MarshalBasicParams(Marshaller marshaller, RateLimitApiOperation element)
    {
        marshaller.Writer.WriteAttribute("calls", element.Calls);
        marshaller.Writer.WriteAttribute("renewal-period", element.RenewalPeriod);
        marshaller.Writer.WriteNullableAttribute("retry-after-header-name", element.RetryAfterHeaderName);
        marshaller.Writer.WriteNullableAttribute("retry-after-variable-name", element.RetryAfterVariableName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-header-name", element.RemainingCallsHeaderName);
        marshaller.Writer.WriteNullableAttribute("remaining-calls-variable-name", element.RemainingCallsVariableName);
        marshaller.Writer.WriteNullableAttribute("total-calls-header-name", element.TotalCallsHeaderName);

    }
}