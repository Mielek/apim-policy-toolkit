
using System.Text;

using Mielek.Builders;
using Mielek.Expressions.Context;
using Mielek.Model;
using Mielek.Model.Attributes;

namespace Mielek.Test;

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

    public PolicyDocument ApiPolicyDocument()
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