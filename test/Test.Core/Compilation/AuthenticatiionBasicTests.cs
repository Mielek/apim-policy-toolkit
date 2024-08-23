namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

[TestClass]
public class AuthenticationBasicTests
{
    [TestMethod]
    public void ShouldCompileAuthenticationBasicPolicyInInboundSections()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context) 
                { 
                    context.AuthenticationBasic("username", "password");
                }
            }
            """;

        var result = code.CompileDocument();

        var expectedXml = """
                          <policies>
                              <inbound>
                                  <authentication-basic username="username" password="password" />
                              </inbound>
                          </policies>
                          """;
        result.Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldAllowExpressionsInAuthenticationBasicPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context) 
                { 
                    context.AuthenticationBasic(Username(context.ExpressionContext), Password(context.ExpressionContext));
                }
            
                public string Username(IExpressionContext context) => context.Subscription.Id;
                public string Password(IExpressionContext context) => context.Subscription.Key;
            }
            """;

        var result = code.CompileDocument();

        var expectedXml =
            """
            <policies>
                <inbound>
                    <authentication-basic username="@(context.Subscription.Id)" password="@(context.Subscription.Key)" />
                </inbound>
            </policies>
            """;
        
        result.Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}