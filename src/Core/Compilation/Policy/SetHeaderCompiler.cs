using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetHeaderCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetHeader);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count != 1)
        {
            context.ReportError("");
            return;
        }

        var value = CompilerUtils.ProcessParameter(context, node.ArgumentList.Arguments[0].Expression);
        context.AddPolicy(new SetBodyPolicyBuilder().Body(value).Build());
    }
}