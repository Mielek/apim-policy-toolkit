using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class ProxyPolicyHandler : MarshallerHandler<ProxyPolicy>
{
    public override void Marshal(Marshaller marshaller, ProxyPolicy element)
    {
        marshaller.Writer.WriteStartElement("proxy");

        marshaller.Writer.WriteAttribute("url", element.Url);
        marshaller.Writer.WriteNullableAttribute("username", element.Username);
        marshaller.Writer.WriteNullableAttribute("password", element.Password);

        marshaller.Writer.WriteEndElement();
    }
}