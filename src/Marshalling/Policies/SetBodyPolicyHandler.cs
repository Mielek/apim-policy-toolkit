using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetBodyPolicyHandler : MarshallerHandler<SetBodyPolicy>
{
    public override void Marshal(Marshaller marshaller, SetBodyPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-body");
        marshaller.Writer.WriteNullableAttribute("template", element.Template);
        marshaller.Writer.WriteNullableAttribute("xsi-nil", element.XsiNil);
        marshaller.Writer.WriteExpression(element.Body);
        marshaller.Writer.WriteEndElement();
    }
}