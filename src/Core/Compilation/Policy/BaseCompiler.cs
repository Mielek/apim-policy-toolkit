using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class BaseCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.Base);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax _)
    {
        context.AddPolicy(new XElement("base"));
    }
}