using System.Text;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Test;

[Library]
public class Library
{

    [Expression]
    public static void Test(Object a, Object b)
    {
        // return context.Timestamp.Ticks;
    }

    [Expression]
    public static void Test(Object c)
    {
        // return context.Timestamp.Ticks;
    }

    [Expression]
    public static string Test(IContext context)
    {
        string[] value;
        if (context.Request.Headers.TryGetValue("Authorization", out value))
        {
            if (value != null && value.Length > 0)
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(value[0]));
            }
        }
        return null;
    }

    public XElement ApiPolicyDocument()
    {
        return PolicyDocumentBuilder.Create()
        .Inbound(i =>
        {
            i.SetHeader(sh =>
            {
                sh.Name(c => c.Lambda(context => context.Api.Id))
                .Value(c => c.Inline(context => context.Product.Id));
            });
        })
        .Build();
    }
}