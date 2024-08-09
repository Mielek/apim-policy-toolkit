using System.Xml.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Tests;

[TestClass]
public class InlinePolicyTests
{
    [TestMethod]
    public void ShouldCompileInlinePolicyInSections()
    {
        var code = CSharpSyntaxTree.ParseText(
            """
                [Document]
                public class PolicyDocument : IDocument
                {
                  public void Inbound(IInboundContext context) 
                  { 
                      context.InlinePolicy("<any-xml />");
                  }
                  public void Outbound(IOutboundContext context)
                  {
                      context.InlinePolicy("<any-xml />");
                  }
                  public void Backend(IBackendContext context) 
                  { 
                      context.InlinePolicy("<any-xml />");
                  }
                  public void OnError(IOnErrorContext context)
                  {
                      context.InlinePolicy("<any-xml />");
                  }
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

        var expectedXml = XElement.Parse(
            """
            <policies>
                <inbound>
                    <any-xml />
                </inbound>
                <outbound>
                    <any-xml />
                </outbound>
                <backend>
                    <any-xml />
                </backend>
                <on-error>
                    <any-xml />
                </on-error>
            </policies>
            """);
        result.Document.Should().BeEquivalentTo(expectedXml);
    }
}