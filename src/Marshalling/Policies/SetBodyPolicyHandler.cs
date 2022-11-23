using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetBodyPolicyHandler : MarshallerHandler<SetBodyPolicy>
{
    public override void Marshal(Marshaller marshaller, SetBodyPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-body");
        if (element.Template != null)
        {
            marshaller.Writer.WriteExpressionAsAttribute("template", element.Template);
        }

        if (element.XsiNil != null)
        {
            marshaller.Writer.WriteExpressionAsAttribute("xsi-nil", element.XsiNil);
        }
        marshaller.Writer.WriteExpression(element.Body);
        marshaller.Writer.WriteEndElement();
    }
}