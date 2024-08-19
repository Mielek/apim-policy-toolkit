using System.Xml.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Tests;

[TestClass]
public class AuthenticationBasicTests
{
    [TestMethod]
    public void ShouldCompileAuthenticationBasicPolicyInInboundSections()
    {
        var code = CSharpSyntaxTree.ParseText(
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context) 
                { 
                    context.AuthenticationBasic("username", "password");
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
                    <authentication-basic username="username" password="password" />
                </inbound>
            </policies>
            """);
        result.Document.Should().BeEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldAllowExpressionsInAuthenticationBasicPolicy()
    {
        var code = CSharpSyntaxTree.ParseText(
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
                    <authentication-basic username="@(context.Subscription.Id)" password="@(context.Subscription.Key)" />
                </inbound>
            </policies>
            """);
        result.Document.Should().BeEquivalentTo(expectedXml);
    }
}