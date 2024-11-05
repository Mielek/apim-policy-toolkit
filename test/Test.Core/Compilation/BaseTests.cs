namespace Azure.ApiManagement.PolicyToolkit.Compilation;

[TestClass]
public class BaseTests
{
    [TestMethod]
    public void ShouldCompileBasePolicyInSections()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context) { context.Base(); }
                public void Outbound(IOutboundContext context) { context.Base(); }
                public void Backend(IBackendContext context) { context.Base(); }
                public void OnError(IOnErrorContext context) { context.Base(); }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <base />
                </inbound>
                <outbound>
                    <base />
                </outbound>
                <backend>
                    <base />
                </backend>
                <on-error>
                    <base />
                </on-error>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}