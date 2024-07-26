using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetBodyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetBody);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for set-body policy. {node.GetLocation()}");
            return;
        }

        var value = CompilerUtils.ProcessParameter(context, node.ArgumentList.Arguments[0].Expression);
        context.AddPolicy(new SetBodyPolicyBuilder().Body(value).Build());
    }
}