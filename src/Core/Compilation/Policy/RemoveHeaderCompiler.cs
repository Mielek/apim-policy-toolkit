using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class RemoveHeaderCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.RemoveHeader);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for set-header policy. {node.GetLocation()}");
            return;
        }
        
        var headerName = CompilerUtils.ProcessParameter(context, node.ArgumentList.Arguments[0].Expression);
        context.AddPolicy(new SetHeaderPolicyBuilder()
            .Name(headerName)
            .ExistsAction(SetHeaderPolicyBuilder.ExistsActionType.Delete)
            .Build());
    }
}