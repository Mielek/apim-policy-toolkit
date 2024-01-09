
using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

public class Test
{
    [Expression]
    public static string FilterBody2(IContext context)
    {
        var response = context.Response.Body.As<JObject>();
        foreach (var key in new[] { "current", "minutely", "hourly", "daily", "alerts" })
        {
            response.Property(key)?.Remove();
        }
        return response.ToString();
    }
}