using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

[TestClass]
public class BaseTests
{
    [TestMethod]
    public void ShouldCompileBasePolicyInSections()
    {
        var code = CSharpSyntaxTree.ParseText(
        """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context) { context.Base(); }
                public void Outbound(IOutboundContext context) { context.Base(); }
                public void Backend(IBackendContext context) { context.Base(); }
                public void OnError(IOnErrorContext context) { context.Base(); }
            }
        """);
        var policy = code
            .GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .First(c => c.AttributeLists.ContainsAttributeOfType("Document"));

        var result = new CSharpPolicyCompiler(policy).Compile();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Errors.Count == 0);
        Assert.IsNotNull(result.Document);

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
        result.Document.Should().BeEquivalentTo(expectedXml);
    }
}