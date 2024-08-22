using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

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

        var expectedXml = 
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
            """;
        result.Document.Should().BeEquivalentTo(expectedXml);
    }
    
    [TestMethod]
    public void ShouldCompileInlinePolicyWithExpressions()
    {
        var code = CSharpSyntaxTree.ParseText(
            """"
                [Document]
                public class PolicyDocument : IDocument
                {
                  public void Inbound(IInboundContext context) 
                  { 
                      context.InlinePolicy("""
                                           <set-header name="@("name" + context.RequestId)" exists-action="override">
                                               <value>1</value>
                                               <value>@{
                                                   return "value" + context.RequestId;
                                               }</value>
                                           </set-header>
                                           """);
                  }
                }
            """");
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
                    <set-header name="@("name" + context.RequestId)" exists-action="override">
                        <value>1</value>
                        <value>@{
                    return "value" + context.RequestId;
                }</value>
                    </set-header>
                </inbound>
            </policies>
            """;
        result.Document.Should().BeEquivalentTo(expectedXml);
    }
    
}