using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetStatusPolicyHandler : MarshallerHandler<SetStatusPolicy>
{
    public override void Marshal(Marshaller marshaller, SetStatusPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-status");
        marshaller.Writer.WriteExpressionAsAttribute("code", element.Code);
        marshaller.Writer.WriteNullableExpressionAsAttribute("reason", element.Reason);
        marshaller.Writer.WriteEndElement();
    }
}