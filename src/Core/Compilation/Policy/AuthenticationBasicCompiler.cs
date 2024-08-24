using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class AuthenticationBasicCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.AuthenticationBasic);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 2)
        {
            context.ReportError($"Wrong argument count for authentication-basic policy. {node.GetLocation()}");
            return;
        }

        var username = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        var password = node.ArgumentList.Arguments[1].Expression.ProcessParameter(context);
        var policy = new AuthenticationBasicPolicyBuilder()
            .Username(username)
            .Password(password)
            .Build();
        context.AddPolicy(policy);
    }
}