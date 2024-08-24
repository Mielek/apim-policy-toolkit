using System.Text;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Test;

[Document("echo-api.retrieve-resource")]
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
    public static string Test(IExpressionContext context)
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
        return Policy.Document()
        .Inbound(i =>
        {
            i.SetHeader(sh =>
            {
                sh.Name(context => context.Api.Id)
                .Value(context => context.Product.Id);
            });
        })
        .Create();
    }
}