using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetHeaderPolicyHandler : MarshallerHandler<SetHeaderPolicy>
{
    public override void Marshal(Marshaller marshaller, SetHeaderPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-header");
        marshaller.Writer.WriteExpressionAsAttribute("name", element.Name);
        if (element.ExistAction != null)
        {
            marshaller.Writer.WriteExpressionAsAttribute("exist-action", element.ExistAction);
        }
        if (element.Values != null)
        {
            foreach (var value in element.Values)
            {
                marshaller.Writer.WriteStartElement("value");
                value.Accept(marshaller);
                marshaller.Writer.WriteEndElement();
            }
        }
        marshaller.Writer.WriteEndElement();
    }
}