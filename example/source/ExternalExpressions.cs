
using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

public static class ExternalExpressions
{
    [Expression]
    public static string FilterBody(IContext context)
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