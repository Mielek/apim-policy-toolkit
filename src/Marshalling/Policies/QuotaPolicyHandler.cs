using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class QuotaPolicyHandler : MarshallerHandler<QuotaPolicy>
{
    public override void Marshal(Marshaller marshaller, QuotaPolicy element)
    {
        marshaller.Writer.WriteStartElement("quota");
        marshaller.Writer.WriteAttribute("renewal-period", element.RenewalPeriod);
        marshaller.Writer.WriteNullableAttribute("calls", element.Calls);
        marshaller.Writer.WriteNullableAttribute("bandwidth", element.Bandwidth);

        if (element.Apis != null && element.Apis.Count > 0)
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

        marshaller.Writer.WriteAttribute("calls", api.Calls);
        marshaller.Writer.WriteNullableAttribute("name", api.Name);
        marshaller.Writer.WriteNullableAttribute("id", api.Id);

        if (api.Operations != null && api.Operations.Count > 0)
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

        marshaller.Writer.WriteAttribute("calls", operation.Calls);
        marshaller.Writer.WriteNullableAttribute("name", operation.Name);
        marshaller.Writer.WriteNullableAttribute("id", operation.Id);

        marshaller.Writer.WriteEndElement();
    }
}