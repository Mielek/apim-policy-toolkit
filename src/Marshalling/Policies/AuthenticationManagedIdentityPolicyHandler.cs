using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class AuthenticationManagedIdentityPolicyHandler : MarshallerHandler<AuthenticationManagedIdentityPolicy>
{
    public override void Marshal(Marshaller marshaller, AuthenticationManagedIdentityPolicy element)
    {
        marshaller.Writer.WriteStartElement("authentication-managed-identity");

        marshaller.Writer.WriteAttribute("resource", element.Resource);
        marshaller.Writer.WriteNullableAttribute("client-id", element.ClientId);
        marshaller.Writer.WriteNullableAttribute("output-token-variable-name", element.OutputTokenVariableName);
        marshaller.Writer.WriteNullableAttribute("ignore-error", element.IgnoreError);

        marshaller.Writer.WriteEndElement();
    }
}