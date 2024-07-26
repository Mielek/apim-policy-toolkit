using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

using Newtonsoft.Json.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

[Document("echo-api.retrieve-resource")]
public class TestingDocument : IDocument
{
    public void Inbound(IInboundContext c)
    {
        c.Base();
        if (IsFromCompanyIp(c.Context))
        {
            c.SetHeader("X-Company", "true");
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
        c.RemoveHeader("Backend-Statistics");
        c.SetBody(FilterRequest(c.Context));
        c.Base();
    }

    bool IsFromCompanyIp(IContext context) => context.Request.IpAddress.StartsWith("10.0.0.");

    string FilterRequest(IContext context)
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