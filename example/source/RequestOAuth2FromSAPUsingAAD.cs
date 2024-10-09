using System.Text;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

[Document]
public class RequestOAuth2FromSAPUsingAAD : IDocument
{
    public void Inbound(IInboundContext context)
    {
        context.Base();
        context.ValidateJwt(new ValidateJwtConfig
        {
            HeaderName = "Authorization",
            FailedValidationHttpCode = 401,
            RequireScheme = "Bearer",
            OpenIdConfigs =
            [
                new OpenIdConfig
                {
                    Url = "https://login.microsoftonline.com/{{AADTenantId}}/.well-known/openid-configuration"
                }
            ],
            Audiences = ["api://{{APIMAADRegisteredAppClientId}}"],
            Issuers = ["https://login.microsoftonline.com/{{AADTenantId}}/v2.0"],
            RequiredClaims =
            [
                new ClaimConfig { Name = "scp", Match = "all", Separator = " ", Values = ["user_impersonation"] }
            ]
        });
        context.SetHeader("Accept-Encoding", "gzip, deflate");
        context.SetVariable("APIMAADRegisteredAppClientId", "{{APIMAADRegisteredAppClientId}}");
        context.SetVariable("APIMAADRegisteredAppClientSecret", "{{APIMAADRegisteredAppClientSecret}}");
        context.SetVariable("AADSAPResource", "{{AADSAPResource}}");
        context.SetVariable("SAPOAuthClientID", "{{SAPOAuthClientID}}");
        context.SetVariable("SAPOAuthClientSecret", "{{SAPOAuthClientSecret}}");
        context.SetVariable("SAPOAuthScope", "{{SAPOAuthScope}}");
        context.SetVariable("SAPOAuthRefreshExpiry", "{{SAPOAuthRefreshExpiry}}");

        context.InlinePolicy(
            "<cache-lookup-value key=\"@(\"SAPPrincipal\" + context.Request.Headers.GetValueOrDefault(\"Authorization\",\"\").AsJwt()?.Subject)\" variable-name=\"SAPBearerToken\" />");
        context.InlinePolicy(
            "<cache-lookup-value key=\"@(\"SAPPrincipalRefresh\" + context.Request.Headers.GetValueOrDefault(\"Authorization\",\"\").AsJwt()?.Subject)\" variable-name=\"SAPRefreshToken\" />");

        if (ContainsSapTokens(context.ExpressionContext))
        {
            context.SendRequest(new SendRequestConfig
            {
                Mode = "new",
                ResponseVariableName = "fetchSAMLAssertion",
                Timeout = 10,
                IgnoreError = false,
                Url = "https://login.microsoftonline.com/{{AADTenantId}}/oauth2/v2.0/token",
                Method = "POST",
                Headers =
                [
                    new HeaderConfig
                    {
                        Name = "Content-Type",
                        ExistsAction = "override",
                        Values = ["application/x-www-form-urlencoded"]
                    }
                ],
                Body = new BodyConfig { Content = CreateAadTokenRequestBody(context.ExpressionContext), }
            });
            context.SetVariable("accessToken", GetTokenFromAadResponse(context.ExpressionContext));
            context.SendRequest(new SendRequestConfig
            {
                Mode = "new",
                ResponseVariableName = "ferchSapBearer",
                Timeout = 10,
                IgnoreError = false,
                Url = "https://{{SAPOAuthServerAdressForTokenEndpoint}}/sap/bc/sec/oauth2/token",
                Method = "POST",
                Headers =
                [
                    new HeaderConfig
                    {
                        Name = "Content-Type",
                        ExistsAction = "override",
                        Values = ["application/x-www-form-urlencoded"]
                    },
                    new HeaderConfig
                    {
                        Name = "Authorization",
                        ExistsAction = "override",
                        Values = [CreateSapAuthorizationHeader(context.ExpressionContext)],
                    },
                    new HeaderConfig { Name = "Ocp-Apim-Subscription-Key", ExistsAction = "Delete" }
                ],
                Body = new BodyConfig { Content = CreateSapTokenRequestBody(context.ExpressionContext), }
            });
        }
    }


    bool ContainsSapTokens(IExpressionContext context)
        => !context.Variables.ContainsKey("SAPBearerToken") && !context.Variables.ContainsKey("SAPRefreshToken");

    string CreateAadTokenRequestBody(IExpressionContext context)
    {
        var _AADRegisteredAppClientId = context.Variables["APIMAADRegisteredAppClientId"];
        var _AADRegisteredAppClientSecret = context.Variables["APIMAADRegisteredAppClientSecret"];
        var _AADSAPResource = context.Variables["AADSAPResource"];
        var assertion = context.Request.Headers.GetValueOrDefault("Authorization", [""])[0].Replace("Bearer ", "");
        return
            $"grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={assertion}&client_id={_AADRegisteredAppClientId}&client_secret={_AADRegisteredAppClientSecret}&scope={_AADSAPResource}/.default&requested_token_use=on_behalf_of&requested_token_type=urn:ietf:params:oauth:token-type:saml2";
    }

    string GetTokenFromAadResponse(IExpressionContext context)
        => (string)((IResponse)context.Variables["fetchSAMLAssertion"]).Body.As<JObject>()["access_token"];

    string CreateSapAuthorizationHeader(IExpressionContext context)
    {
        var _SAPOAuthClientID = context.Variables["SAPOAuthClientID"];
        var _SAPOAuthClientSecret = context.Variables["SAPOAuthClientSecret"];
        return "Basic " +
               Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_SAPOAuthClientID}:{_SAPOAuthClientSecret}"));
    }

    string CreateSapTokenRequestBody(IExpressionContext context)
    {
        var _SAPOAuthClientID = context.Variables["SAPOAuthClientID"];
        var _SAPOAuthScope = context.Variables["SAPOAuthScope"];
        var assertion2 = context.Variables["accessToken"];
        return
            $"grant_type=urn:ietf:params:oauth:grant-type:saml2-bearer&assertion={assertion2}&client_id={_SAPOAuthClientID}&scope={_SAPOAuthScope}";
    }
}