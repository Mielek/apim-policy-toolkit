using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetHeaderPolicyHandler : MarshallerHandler<SetHeaderPolicy>
{
    public override void Marshal(Marshaller marshaller, SetHeaderPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-header");
        marshaller.Writer.WriteAttribute("name", element.Name);
        marshaller.Writer.WriteNullableAttribute("exists-action", element.ExistsAction);
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