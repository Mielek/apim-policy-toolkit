using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class ReturnResponsePolicyHandler : MarshallerHandler<ReturnResponsePolicy>
{
    public override void Marshal(Marshaller marshaller, ReturnResponsePolicy element)
    {
        marshaller.Writer.WriteStartElement("return-response");
        marshaller.Writer.WriteNullableAttribute("response-variable-name", element.ResponseVariableName);

        element.SetHeaderPolicy?.Accept(marshaller);
        element.SetBodyPolicy?.Accept(marshaller);
        element.SetStatusPolicy?.Accept(marshaller);

        marshaller.Writer.WriteEndElement();
    }
}