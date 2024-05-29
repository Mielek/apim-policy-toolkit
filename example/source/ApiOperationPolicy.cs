using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

[CodeDocument("echo-api.retrieve-resource")]
public class ApiOperationPolicy : ICodeDocument
{
    public void Inbound(IInboundContext c)
    {
        c.Base();
        if (IsCompanyIP(c.Context))
        {
            c.SetHeader("X-Company", "true");
            c.AuthenticationBasic("{{username}}", "{{password}}");
        }
        else
        {
            var msiToken = c.AuthenticationManagedIdentity("resource-id");
            c.SetHeader("Authorization", $"Bearer {msiToken}");
        }
    }

    public void Outbound(IOutboundContext c)
    {
        c.RemoveHeader("Company-stats");
        c.SetBody(FilterSecrets(c.Context));
    }

    [Expression]
    public bool IsCompanyIP(IContext context)
        => context.Request.IpAddress.StartsWith("10.0.0.");


    [Expression]
    public string FilterSecrets(IContext context)
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