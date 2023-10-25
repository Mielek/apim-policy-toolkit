using Mielek.Model;
using Mielek.Model.Attributes;
using Mielek.Builders;
using Mielek.Model.Policies;
using Mielek.Expressions.Context;
using Newtonsoft.Json.Linq;

namespace Contoso.Apis;

[Library]
public class EchoApi
{

    [Document]
    public PolicyDocument ApiPolicyDocument()
    {
        return PolicyDocumentBuilder.Create()
            .Inbound(policies =>
            {
                policies
                    .CheckHeader(policy =>
                    {
                        policy.Name("X-Checked")
                            .FailedCheckHttpCode(400)
                            .FailedCheckErrorMessage("Bad request")
                            .IgnoreCase(expression => expression.Method(IsVariableSet))
                            .Value("Test")
                            .Value("Other-Test");
                    })
                    .Base()
                    .SetHeader(policy =>
                    {
                        policy.Name("X-Test").ExistsAction(SetHeaderExistsAction.Append)
                            .Value("Test")
                            .Value(expression => expression.Inline(context => context.Deployment
                            .Region))
                            .Value(expression => expression.Lambda(context =>
                            {
                                if (context.Variables.ContainsKey("Variable"))
                                {
                                    return "ContainsVariable";
                                }
                                return "NotContainVariable";
                            }))
                            .Value(expression => expression.Method(GetKnownGUIDOrGenerateNew));
                    });
            })
            .Outbound(policies => policies.Base().SetBody(policy => policy.Body(expression => expression.Method(FilterBody))))
            .Build();
    }

    public bool IsVariableSet(IContext context) => context.Variables.ContainsKey("Variable");

    public string GetKnownGUIDOrGenerateNew(IContext context)
    {
        if (!context.Variables
        .TryGetValue("KnownGUID", out var guid))
        {
            guid = Guid.NewGuid();
        }
        return guid.ToString();
    }

    [Expression]
    public string FilterBody(IContext context)
    {
        var response = context.Response.Body.As<JObject>();
        foreach (var key in new[] { "current", "minutely", "hourly", "daily", "alerts" })
        {
            response.Property(key)?.Remove();
        }
        return response.ToString();
    }

}