using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

[Document("echo-api.retrieve-resource")]
public class ApiOperationPolicy : IDocument
{
    public void Inbound(IInboundContext c)
    {
        c.Base();
        if (IsFromCompanyIp(c.ExpressionContext))
        {
            c.Cors(new CorsConfig
            {
                AllowCredentials = true,
                AllowedOrigins = ["http://internal.contoso.example"],
                AllowedHeaders = ["*"],
                AllowedMethods = ["*"],
                ExposeHeaders = ["*"],
            });
            c.InlinePolicy("<set-backend-service base-url=\"https://internal.contoso.example\" />");
            c.AuthenticationBasic("{{username}}", "{{password}}");
        }
        else
        {
            c.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
            {
                Resource = "https://management.azure.com/", 
                OutputTokenVariableName = "testToken",
            });
            c.SetHeader("Authorization", Bearer(c.ExpressionContext));
        }
    }

    public void Outbound(IOutboundContext c)
    {
        c.Base();
        c.SetBody(FilterSecrets(c.ExpressionContext));
    }

    public bool IsFromCompanyIp(IExpressionContext context)
        => context.Request.IpAddress.StartsWith("10.0.0.");

    public string Bearer(IExpressionContext context)
        => $"Bearer {context.Variables["testToken"]}";

    [Expression]
    public string FilterSecrets(IExpressionContext context)
    {
        var body = context.Response.Body.As<JObject>();
        foreach (var internalProperty in new string[] { "location", "secret" })
        {
            if (body.ContainsKey(internalProperty))
            {
                body.Remove(internalProperty);
            }
        }

        return body.ToString();
    }
}