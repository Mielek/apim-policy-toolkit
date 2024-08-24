using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class AuthenticationManageIdentityCompiler : IReturnValueMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.AuthenticationManagedIdentity);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node, string variableName)
    {
        var resource = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        var policy = new AuthenticationManagedIdentityPolicyBuilder()
            .Resource(resource)
            .OutputTokenVariableName(variableName)
            .Build();
        context.AddPolicy(policy);
    }
}