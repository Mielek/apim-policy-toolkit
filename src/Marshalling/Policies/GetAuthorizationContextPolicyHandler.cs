using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class GetAuthorizationContextPolicyHandler : MarshallerHandler<GetAuthorizationContextPolicy>
{
    public override void Marshal(Marshaller marshaller, GetAuthorizationContextPolicy element)
    {
        marshaller.Writer.WriteStartElement("get-authorization-context");

        marshaller.Writer.WriteAttribute("provider-id", element.ProviderId);
        marshaller.Writer.WriteAttribute("authorization-id", element.AuthorizationId);
        marshaller.Writer.WriteAttribute("context-variable", element.ContextVariableName);

        marshaller.Writer.WriteNullableAttribute("identity-type", TranslateIdentity(element.IdentityType));
        marshaller.Writer.WriteNullableAttribute("identity", element.Identity);
        marshaller.Writer.WriteNullableAttribute("ignore-error", element.IgnoreError);

        marshaller.Writer.WriteEndElement();
    }

    private static string? TranslateIdentity(IdentityType? identityType) => identityType switch
    {
        null => null,
        IdentityType.Managed => "managed",
        IdentityType.JWT => "jwt",
        _ => throw new NotImplementedException(),
    };
}