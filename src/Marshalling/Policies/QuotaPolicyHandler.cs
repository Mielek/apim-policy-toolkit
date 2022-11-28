using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class QuotaPolicyHandler : MarshallerHandler<QuotaPolicy>
{
    public override void Marshal(Marshaller marshaller, QuotaPolicy element)
    {
        marshaller.Writer.WriteStartElement("quota");
        marshaller.Writer.WriteAttribute("renewal-period", $"{element.RenewalPeriod}");
        if (element.Calls != null)
        {
            marshaller.Writer.WriteAttribute("calls", $"{element.Calls}");
        }
        if (element.Bandwidth != null)
        {
            marshaller.Writer.WriteAttribute("bandwidth", $"{element.Bandwidth}");
        }
        if (element.Apis != null)
        {
            foreach (var api in element.Apis)
            {
                MarshalApi(marshaller, api);
            }
        }

        marshaller.Writer.WriteEndElement();
    }

    private void MarshalApi(Marshaller marshaller, QuotaApi api)
    {
        marshaller.Writer.WriteStartElement("api");

        marshaller.Writer.WriteAttribute("calls", $"{api.Calls}");
        if (api.Name != null) marshaller.Writer.WriteAttribute("name", $"{api.Name}");
        if (api.Id != null) marshaller.Writer.WriteAttribute("id", $"{api.Id}");

        if (api.Operations != null)
        {
            foreach (var operation in api.Operations)
            {
                MarshalOperation(marshaller, operation);
            }
        }

        marshaller.Writer.WriteEndElement();
    }

    private void MarshalOperation(Marshaller marshaller, QuotaOperation operation)
    {
        marshaller.Writer.WriteStartElement("operation");

        marshaller.Writer.WriteAttribute("calls", $"{operation.Calls}");
        if (operation.Name != null) marshaller.Writer.WriteAttribute("name", $"{operation.Name}");
        if (operation.Id != null) marshaller.Writer.WriteAttribute("id", $"{operation.Id}");

        marshaller.Writer.WriteEndElement();
    }
}