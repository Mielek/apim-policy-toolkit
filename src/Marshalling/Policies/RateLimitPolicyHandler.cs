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
        marshaller.Writer.WriteAttribute("name", element.Name);
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
        marshaller.Writer.WriteAttribute("name", element.Name);
        MarshalBasicParams(marshaller, element);
        marshaller.Writer.WriteEndElement();
    }

    public void MarshalBasicParams(Marshaller marshaller, RateLimitPolicy element)
    {
        marshaller.Writer.WriteAttribute("calls", $"{element.Calls}");
        marshaller.Writer.WriteAttribute("renewal-period", $"{element.RenewalPeriod}");
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
    }
    public void MarshalBasicParams(Marshaller marshaller, RateLimitApi element)
    {
        marshaller.Writer.WriteAttribute("calls", $"{element.Calls}");
        marshaller.Writer.WriteAttribute("renewal-period", $"{element.RenewalPeriod}");
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
    }
    public void MarshalBasicParams(Marshaller marshaller, RateLimitApiOperation element)
    {
        marshaller.Writer.WriteAttribute("calls", $"{element.Calls}");
        marshaller.Writer.WriteAttribute("renewal-period", $"{element.RenewalPeriod}");
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
    }
}