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
        if(IsFromCompanyIp(c.Context))
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
        c.SetBody(FilterSecrets(c.Context));
    }

    public bool IsFromCompanyIp(IContext context)
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