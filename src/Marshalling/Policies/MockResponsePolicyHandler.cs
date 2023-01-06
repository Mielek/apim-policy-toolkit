using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class MockResponsePolicyHandler : MarshallerHandler<MockResponsePolicy>
{
    public override void Marshal(Marshaller marshaller, MockResponsePolicy element)
    {
        marshaller.Writer.WriteStartElement("mock-response");

        marshaller.Writer.WriteNullableAttribute("status-code", element.StatusCode);
        marshaller.Writer.WriteNullableAttribute("content-type", element.ContentType);

        marshaller.Writer.WriteEndElement();
    }
}