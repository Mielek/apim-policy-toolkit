using System.Text;
using System.Xml;

using Mielek.Builders;
using Mielek.Builders.Policies;

namespace Builders;

[TestClass]
public class PolicyDocumentBuilderTest
{
    [TestMethod]
    public void TestMethod1()
    {
        var xml = PolicyDocumentBuilder.Create()
            .Inbound(policies =>
            {
                policies
                    .CheckHeader(policy =>
                    {
                        policy.Name("X-Checked")
                            .FailedCheckHttpCode(400)
                            .FailedCheckErrorMessage("Bad request")
                            .IgnoreCase(expression => expression.Inline(context => context.RequestId.ToString() == "123"))
                            .Value("Test")
                            .Value("Other-Test");
                    })
                    .Base()
                    .SetHeader(policy =>
                    {
                        policy.Name("X-Test").ExistsAction(SetHeaderPolicyBuilder.SetHeaderPolicyExistsAction.Append)
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
                            }));
                        // .Value(expression => expression.Method(GetKnownGUIDOrGenerateNew));
                    });
            })
            // .Outbound(policies => policies.Base().SetBody(policy => policy.Body(expression => expression.Method(FilterBody))))
            .Build();

        throw new Exception(xml.ToString());
    }
}