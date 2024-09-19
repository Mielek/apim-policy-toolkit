using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

[TestClass]
public class Test
{
    [TestMethod]
    public void TestMethod1()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            """
            using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
            using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

            using Newtonsoft.Json.Linq;

            namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

            [Document("echo-api.retrieve-resource")]
            public class TestingDocument : IDocument
            {
                public void Inbound(IInboundContext c)
                {
                    c.Base();
                    if (IsFromCompanyIp(c.ExpressionContext))
                    {
                        c.SetHeader("X-Company", "true");
                        c.AuthenticationBasic("{{username}}", "{{password}}");
                    }
                    else if(IsInternalIp(c.ExpressionContext))
                    {
                        c.AuthenticationBasic("{{username}}", "{{password}}");
                    }
                    else 
                    {
                        var testToken = c.AuthenticationManagedIdentity("test");
                        c.SetHeader("Authorization", $"Bearer {testToken}");
                    }
                }
            
                public void Outbound(IOutboundContext c)
                {
                    c.RemoveHeader("Backend-Statistics");
                    c.SetBody(FilterRequest(c.ExpressionContext));
                    c.Base();
                }
                
                bool IsFromCompanyIp(IExpressionContext context) => context.Request.IpAddress.StartsWith("10.0.0.");
                
                bool IsInternalIp(IExpressionContext context) => context.Request.IpAddress.StartsWith("10.1.0.");
            
                string FilterRequest(IExpressionContext context)
                {
                    var body = context.Response.Body.As<JObject>();
                    foreach (var internalProperty in new string[]{ "location", "secret" })
                    {
                        if (body.ContainsKey(internalProperty))
                        {
                            body.Remove(internalProperty);
                        }
                    }
                    return body.ToString();
                }

            }
            """);
        var policy = syntaxTree
            .GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .First(c => c.AttributeLists.ContainsAttributeOfType("Document"));
        var result = new CSharpPolicyCompiler(policy).Compile();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Errors.Count == 0, string.Join(",", result.Errors));
        Assert.IsNotNull(result.Document);
    }
}