using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class ValidateAzureAdTokenPolicyHandler : MarshallerHandler<ValidateAzureAdTokenPolicy>
{
    public override void Marshal(Marshaller marshaller, ValidateAzureAdTokenPolicy element)
    {
        marshaller.Writer.WriteStartElement("validate-azure-ad-token");
        marshaller.Writer.WriteNullableAttribute("tenant-id", element.TenantId);
        marshaller.Writer.WriteNullableAttribute("header-name", element.HeaderName);
        marshaller.Writer.WriteNullableAttribute("query-parameter-name", element.QueryParameterName);
        marshaller.Writer.WriteNullableAttribute("token-value", element.TokenValue);
        marshaller.Writer.WriteNullableAttribute("failed-validation-httpcode", element.FailedValidationHttpCode);
        marshaller.Writer.WriteNullableAttribute("failed-validation-error-message", element.FailedValidationErrorMessage);
        marshaller.Writer.WriteNullableAttribute("output-token-variable-name", element.OutputTokenVariableName);

        marshaller.Writer.WriteElementCollection("client-application-ids", "application-id", element.ClientApplicationIds);
        marshaller.Writer.WriteNullableElementCollection("backend-application-ids", "application-id", element.BackendApplicationIds);
        marshaller.Writer.WriteNullableElementCollection("audiences", "audience", element.Audiences);

        MarshalRequiredClaims(marshaller, element.RequiredClaims);

        marshaller.Writer.WriteEndElement();
    }

    private void MarshalRequiredClaims(Marshaller marshaller, ICollection<ValidateAzureAdTokenClaim>? requiredClaims)
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

    private string? TranslateClaimMatch(ValidateAzureAdTokenClaimMatch? match) => match switch
    {
        null => null,
        ValidateAzureAdTokenClaimMatch.All => "all",
        ValidateAzureAdTokenClaimMatch.Any => "any",
        _ => throw new NotImplementedException(),
    };
}
