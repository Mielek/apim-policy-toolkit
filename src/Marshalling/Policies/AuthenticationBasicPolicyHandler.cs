using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class AuthenticationBasicPolicyHandler : MarshallerHandler<AuthenticationBasicPolicy>
{
    public override void Marshal(Marshaller marshaller, AuthenticationBasicPolicy element)
    {
        marshaller.Writer.WriteStartElement("authentication-basic");

        marshaller.Writer.WriteAttribute("username", element.Username);
        marshaller.Writer.WriteAttribute("password", element.Password);

        marshaller.Writer.WriteEndElement();
    }
}