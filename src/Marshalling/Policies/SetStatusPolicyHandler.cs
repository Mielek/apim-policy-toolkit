using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetStatusPolicyHandler : MarshallerHandler<SetStatusPolicy>
{
    public override void Marshal(Marshaller marshaller, SetStatusPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-status");
        marshaller.Writer.WriteAttribute("code", element.Code);
        marshaller.Writer.WriteNullableAttribute("reason", element.Reason);
        marshaller.Writer.WriteEndElement();
    }
}