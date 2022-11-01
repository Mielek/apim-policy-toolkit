using System.Xml;

using Mielek.Marshalling;
using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class GetAuthorizationContextPolicyHandler : MarshallerHandler<GetAuthorizationContextPolicy>
{
    public override void Marshal(Marshaller marshaller, GetAuthorizationContextPolicy element)
    {
        marshaller.Writer.WriteStartElement("get-authorization-context");

        marshaller.Writer.WriteExpressionAsAttribute("provider-id", element.ProviderId);
        marshaller.Writer.WriteExpressionAsAttribute("authorization-id", element.AuthorizationId);
        marshaller.Writer.WriteAttribute("context-variable", element.ContextVariableName);

        if (element.IdentityType.HasValue) marshaller.Writer.WriteAttribute("identity-type", TranslateIdentity(element.IdentityType.Value));
        if (element.Identity != null) marshaller.Writer.WriteExpressionAsAttribute("identity", element.Identity);
        if (element.IgnoreError.HasValue) marshaller.Writer.WriteAttribute("ignore-error", element.IgnoreError.Value.ToString());

        marshaller.Writer.WriteEndElement();
    }

    static string TranslateIdentity(IdentityType identityType) => identityType switch
    {
        IdentityType.Managed => "managed",
        IdentityType.JWT => "jwt",
        _ => throw new NotImplementedException(),
    };
}