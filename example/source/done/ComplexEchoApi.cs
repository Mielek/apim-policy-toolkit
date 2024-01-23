using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

using Newtonsoft.Json.Linq;

using System.Xml.Linq;

using static Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetHeaderPolicyBuilder;

namespace Contoso.Apis;

[Library(Name = "echo-api")]
public class ComplexEchoApi
{
    [Document(Name = "modify-resource")]
    public XElement RetrieveResource()
    {
        return Policy.Document()
            .Inbound(policies => policies
                .Base()
                .Choose(c => c
                    .When(w => w
                        .Condition(context => context.Request.IpAddress.StartsWith("10.0.0."))
                        .Policies(p => p
                            .SetHeader(s => s.Name("X-Company").Value("true"))
                            .AuthenticationBasic(a => a.Username("{{username}}").Password("{{password}}"))
                        )
                    )
                    .Otherwise(o => o
                        .AuthenticationManagedIdentity(a => a.Resource("resource").OutputTokenVariableName("token"))
                        .SetHeader(s =>
                            s.Name("Authorization").Value(context => $"Bearer {context.Variables["token"]}")
                        )
                    )
                )
            )
            .Outbound(policies => policies
                .SetHeader(s => s.Name("Backend-Statistics").ExistsAction(ExistsActionType.Delete))
                .SetBody(policy => policy.Body(FilterBody))
                .Base()
            )
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