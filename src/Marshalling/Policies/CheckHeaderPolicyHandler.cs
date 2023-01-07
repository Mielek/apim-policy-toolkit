using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class CheckHeaderPolicyHandler : MarshallerHandler<CheckHeaderPolicy>
{
    public override void Marshal(Marshaller marshaller, CheckHeaderPolicy element)
    {
        marshaller.Writer.WriteStartElement("check-header");

        marshaller.Writer.WriteAttribute("name", element.Name);
        marshaller.Writer.WriteAttribute("failed-check-httpcode", element.FailedCheckHttpCode);
        marshaller.Writer.WriteAttribute("failed-check-error-message", element.FailedCheckErrorMessage);
        marshaller.Writer.WriteAttribute("ignore-case", element.IgnoreCase);

        foreach (var value in element.Values)
        {
            marshaller.Writer.WriteElement("value", value);
        }

        marshaller.Writer.WriteEndElement();
    }
}