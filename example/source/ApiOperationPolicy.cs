using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

[Document("echo-api.retrieve-resource")]
public class ApiOperationPolicy : IDocument
{
    public void Inbound(IInboundContext c)
    {
        c.Base();
        if(IsFromCompanyIp(c.ExpressionContext))
        {
            c.AuthenticationBasic("{{username}}", "{{password}}");
        }
        else
        {
            var testToken = c.AuthenticationManagedIdentity("test");
            c.SetHeader("Authorization", $"Bearer {testToken}");
        }
    }

    public void Outbound(IOutboundContext c)
    {
        c.Base();
        c.SetBody(FilterSecrets(c.ExpressionContext));
    }

    public bool IsFromCompanyIp(IExpressionContext context)
        => context.Request.IpAddress.StartsWith("10.0.0.");


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