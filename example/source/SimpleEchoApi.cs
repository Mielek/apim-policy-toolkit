using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

[Library(Name = "echo-api")]
public class SimpleEchoApi
{
    [Document(Name = "retrieve-resource")]
    public XElement RetrieveResourcePolicyDocument()
    {
        return Policy.Document()
            .Outbound(o => o
                .Base()
                .SetBody(p => p.Body(FilterBody)))
            .Create();
    }

    [Expression]
    public string FilterBody(IContext context)
    {
        var body = context.Response.Body.As<JObject>();
        foreach (var internalProperty in new string[]{ "location", "secret" })
        {
            if (body.ContainsKey(internalProperty))
            {
                body.Remove(internalProperty);
            }
        }
        return body.ToString();
    }
}