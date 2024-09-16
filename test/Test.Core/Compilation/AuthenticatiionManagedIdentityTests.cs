namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

[TestClass]
public class AuthenticationManagedIdentityTests
{
    [TestMethod]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context) 
            { 
                context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig { Resource = "resource" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <authentication-managed-identity resource="resource" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile authentication manage identity policy"
    )]
    public void ShouldCompileAuthenticationManagedIdentityPolicy(string code, string expectedXml)
    {
        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}