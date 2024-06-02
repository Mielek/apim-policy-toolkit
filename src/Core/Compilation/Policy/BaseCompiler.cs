using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class BaseCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.Base);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax _)
    {
        context.AddPolicy(new XElement("base"));
    }
}