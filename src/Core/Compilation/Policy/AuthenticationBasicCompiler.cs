using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class AuthenticationBasicCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.AuthenticationBasic);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var username = CompilerUtils.ProcessParameter(context, node.ArgumentList.Arguments[0].Expression);
        var password = CompilerUtils.ProcessParameter(context, node.ArgumentList.Arguments[1].Expression);
        var policy = new AuthenticationBasicPolicyBuilder()
            .Username(username)
            .Password(password)
            .Build();
        context.AddPolicy(policy);
    }
}