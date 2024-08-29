namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

[TestClass]
public class SendRequestTests
{
    [TestMethod]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendRequest(new SendRequestConfig {
                    ResponseVariableName = "variable",
                    Mode = "new",
                    Timeout = 100,
                    IgnoreError = false,
                    Url = "https://test.example",
                    Method = "POST",
                    Headers = [
                        new HeaderConfig {
                            Name = "content-type",
                            Values = ["plain/text"],
                        },
                        new HeaderConfig {
                            Name = "accept",
                            Values = ["application/json", "application/xml"],
                        },
                    ],
                    Body = new BodyConfig {
                        Content = "body",
                    },
                    Authentication = new BasicAuthenticationConfig {
                        Username = "user",
                        Password = "password",
                    },
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-request response-variable-name="variable" mode="new" timeout="100" ignore-error="false">
                    <set-url>https://example.com</set-url>
                    <set-method>POST</set-method>
                    <set-header name="content-type">
                        <value>plain/text</value>
                    </set-header>
                    <set-header name="accept">
                        <value>application/json</value>
                        <value>application/xml</value>
                    </set-header>
                    <set-body>body</set-body>
                </send-request>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send request policy"
    )]
    public void ShouldCompileSendRequestPolicy(string code, string expectedXml)
    {
        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}