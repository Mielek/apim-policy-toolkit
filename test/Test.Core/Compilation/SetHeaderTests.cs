using System.Text;
using System.Xml;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;
using Mielek.Azure.ApiManagement.PolicyToolkit.Serialization;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Tests;

[TestClass]
public class SetHeaderCompilationTests
{
    [TestMethod]
    [DataRow("SetHeader", "override")]
    [DataRow("AppendHeader", "append")]
    [DataRow("SetHeaderIfNotExist", "skip")]
    public void ShouldCompileSetHeaderPolicyInSections(string method, string type)
    {
        var code = CSharpSyntaxTree.ParseText(
            $$"""
                  [Document]
                  public class PolicyDocument : IDocument
                  {
                      public void Inbound(IInboundContext context) 
                      { 
                          context.{{method}}("X-Header", "1");
                      }
                      public void Outbound(IOutboundContext context)
                      {
                          context.{{method}}("X-Header", "1");
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
            $$"""
              <policies>
                  <inbound>
                      <set-header name="X-Header" exists-action="{{type}}">
                          <value>1</value>
                      </set-header>
                  </inbound>
                  <outbound>
                      <set-header name="X-Header" exists-action="{{type}}">
                          <value>1</value>
                      </set-header>
                  </outbound>
              </policies>
              """);
        result.Document.Should().BeEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldCompileRemoveHeaderPolicyInSections()
    {
        var code = CSharpSyntaxTree.ParseText(
            """
                [Document]
                public class PolicyDocument : IDocument
                {
                    public void Inbound(IInboundContext context) 
                    {
                        context.RemoveHeader("Delete");
                    }
                    public void Outbound(IOutboundContext context)
                    {
                        context.RemoveHeader("Delete");
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
                    <set-header name="Delete" exists-action="delete" />
                </inbound>
                <outbound>
                    <set-header name="Delete" exists-action="delete" />
                </outbound>
            </policies>
            """);
        result.Document.Should().BeEquivalentTo(expectedXml);
    }

    [TestMethod]
    [DataRow("SetHeader", "override")]
    [DataRow("AppendHeader", "append")]
    [DataRow("SetHeaderIfNotExist", "skip")]
    public void ShouldCompileSetHeaderPolicyWithMultipleParameters(string method, string type)
    {
        var code = CSharpSyntaxTree.ParseText(
            $$"""
                  [Document]
                  public class PolicyDocument : IDocument
                  {
                      public void Inbound(IInboundContext context) 
                      {
                          context.{{method}}("X-Header", "1", "2", "3");
                      }
                      public void Outbound(IOutboundContext context)
                      {
                          context.{{method}}("X-Header", "3", "2", "1");
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
            $$"""
              <policies>
                  <inbound>
                      <set-header name="X-Header" exists-action="{{type}}">
                          <value>1</value>
                          <value>2</value>
                          <value>3</value>
                      </set-header>
                  </inbound>
                  <outbound>
                      <set-header name="X-Header" exists-action="{{type}}">
                          <value>3</value>
                          <value>2</value>
                          <value>1</value>
                      </set-header>
                  </outbound>
              </policies>
              """);
        result.Document.Should().BeEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldCompileSetHeaderPolicyWithPolicyExpressionInName()
    {
        var code = CSharpSyntaxTree.ParseText(
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context) 
                {
                    context.SetHeader(NameFromExpression(context.ExpressionContext), "1");
                }
                public void Outbound(IOutboundContext context)
                {
                    context.SetHeader(NameFromExpression(context.ExpressionContext), "1");
                }
            
                public string NameFromExpression(IExpressionContext context) => "name" + context.RequestId;
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
                    <set-header name="@("name" + context.RequestId)" exists-action="override">
                        <value>1</value>
                    </set-header>
                </inbound>
                <outbound>
                    <set-header name="@("name" + context.RequestId)" exists-action="override">
                        <value>1</value>
                    </set-header>
                </outbound>
            </policies>
            """;

        result.Document.Should().BeEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldCompileSetHeaderPolicyWithPolicyExpressionInValue()
    {
        var code = CSharpSyntaxTree.ParseText(
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context) 
                {
                    context.SetHeader("X-Header", "1", ValueFromExpression(context.ExpressionContext), "2");
                }
                public void Outbound(IOutboundContext context)
                {
                    context.SetHeader("X-Header", "1", ValueFromExpression(context.ExpressionContext), "2");
                }
            
                public string ValueFromExpression(IExpressionContext context) => "value" + context.RequestId;
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
                    <set-header name="X-Header" exists-action="override">
                        <value>1</value>
                        <value>@("value" + context.RequestId)</value>
                        <value>2</value>
                    </set-header>
                </inbound>
                <outbound>
                    <set-header name="X-Header" exists-action="override">
                        <value>1</value>
                        <value>@("value" + context.RequestId)</value>
                        <value>2</value>
                    </set-header>
                </outbound>
            </policies>
            """);

        result.Document.Should().BeEquivalentTo(expectedXml);
    }
}