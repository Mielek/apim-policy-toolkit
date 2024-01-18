using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

using Newtonsoft.Json.Linq;

using System.Xml.Linq;

using static Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetHeaderPolicyBuilder;

namespace Contoso.Apis;

[Library(Name = "echo-api")]
public class EchoApi
{
    [Document(Name = "retrieve-resource")]
    public XElement RetrieveResource()
    {
        return Policy.Document()
            .Inbound(policies =>
            {
                policies
                    .CheckHeader(policy =>
                        policy.Name("X-Checked")
                            .FailedCheckHttpCode(400)
                            .FailedCheckErrorMessage("Bad request")
                            .IgnoreCase(IsVariableSet)
                            .Value("Test")
                            .Value("Other-Test"))
                    .Base()
                    .SetHeader(policy =>
                        policy.Name("X-Test").ExistsAction(ExistsActionType.Append)
                            .Value("Test")
                            .Value(context => context.Deployment.Region)
                            .Value((context) =>
                            {
                                if (context.Variables.ContainsKey("Variable"))
                                {
                                    return "ContainsVariable";
                                }

                                return "NotContainVariable";
                            })
                            .Value(GetKnownGUIDOrGenerateNew));
            })
            .Outbound(policies => policies.Base().SetBody(policy => policy.Body(Test.FilterBody2)))
            .Create();
    }

    [Expression]
    public bool IsVariableSet(IContext context) => context.Variables.ContainsKey("Variable");

    [Expression]
    public string GetKnownGUIDOrGenerateNew(IContext context)
    {
        if (!context.Variables
                .TryGetValue("KnownGUID", out var guid))
        {
            guid = Guid.NewGuid();
        }

        return $"{guid}";
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