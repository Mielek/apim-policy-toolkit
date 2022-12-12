using Mielek.Model.Expressions;
using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class ValidateJwtPolicyHandler : MarshallerHandler<ValidateJwtPolicy>
{
    public override void Marshal(Marshaller marshaller, ValidateJwtPolicy element)
    {
        marshaller.Writer.WriteStartElement("validate-jwt");
        marshaller.Writer.WriteNullableAttribute("header-name", element.HeaderName);
        marshaller.Writer.WriteNullableAttribute("query-parameter-name", element.QueryParameterName);
        marshaller.Writer.WriteNullableAttribute("token-value", element.TokenValue);
        marshaller.Writer.WriteNullableAttribute("failed-validation-httpcode", element.FailedValidationHttpCode);
        marshaller.Writer.WriteNullableAttribute("failed-validation-error-message", element.FailedValidationErrorMessage);
        marshaller.Writer.WriteNullableAttribute("require-expiration-time", element.RequireExpirationTime);
        marshaller.Writer.WriteNullableAttribute("require-scheme", element.RequireScheme);
        marshaller.Writer.WriteNullableAttribute("require-signed-tokens", element.RequireSignedTokens);
        marshaller.Writer.WriteNullableAttribute("clock-skew", element.ClockSkew);
        marshaller.Writer.WriteNullableAttribute("output-token-variable-name", element.OutputTokenVariableName);

        
        marshaller.Writer.WriteNullableElementCollection("issuer-signing-keys", "key", element.IssuerSigningKeys);
        marshaller.Writer.WriteNullableElementCollection("decryption-keys", "key", element.DecryptionKeys);
        marshaller.Writer.WriteNullableElementCollection("audiences", "audience", element.Audiences);
        marshaller.Writer.WriteNullableElementCollection("issuers", "issuer", element.Issuers);

        MarshalRequiredClaims(marshaller, element.RequiredClaims);

        marshaller.Writer.WriteEndElement();
    }

    private void MarshalRequiredClaims(Marshaller marshaller, ICollection<ValidateJwtClaim>? requiredClaims)
    {
        if (requiredClaims == null || requiredClaims.Count == 0) return;

        marshaller.Writer.WriteStartElement("required-claims");
        foreach (var requiredClaim in requiredClaims)
        {
            marshaller.Writer.WriteStartElement("claim");
            marshaller.Writer.WriteAttribute("name", requiredClaim.Name);
            marshaller.Writer.WriteNullableAttribute("match", TranslateClaimMatch(requiredClaim.Match));
            marshaller.Writer.WriteNullableAttribute("separator", requiredClaim.Separator);

            foreach (var value in requiredClaim.Values)
            {
                marshaller.Writer.WriteElement("value", value);
            }
            marshaller.Writer.WriteEndElement();
        }
        marshaller.Writer.WriteEndElement();
    }

    private string? TranslateClaimMatch(ValidateJwtClaimMatch? match) => match switch
    {
        null => null,
        ValidateJwtClaimMatch.All => "all",
        ValidateJwtClaimMatch.Any => "any",
        _ => throw new NotImplementedException(),
    };
}
