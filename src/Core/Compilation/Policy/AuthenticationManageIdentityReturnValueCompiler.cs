using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class AuthenticationManageIdentityReturnValueCompiler : IReturnValueMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.AuthenticationManagedIdentity);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node, string variableName)
    {
        var policy = new XElement("authentication-managed-identity");
        var resource = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        policy.Add(new XAttribute("resource", resource));
        policy.Add(new XAttribute("output-token-variable-name", variableName));

        context.AddPolicy(policy);
    }
}