using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetHeaderCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetHeader);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count < 2)
        {
            context.ReportError($"Wrong argument count for set-header policy. {node.GetLocation()}");
            return;
        }
        
        var headerName = CompilerUtils.ProcessParameter(context, node.ArgumentList.Arguments[0].Expression);
        var builder = new SetHeaderPolicyBuilder()
            .Name(headerName)
            .ExistsAction(SetHeaderPolicyBuilder.ExistsActionType.Override);

        for (int i = 1; i < arguments.Count; i++)
        {
            builder.Value(CompilerUtils.ProcessParameter(context, arguments[i].Expression));
        }

        var policy = builder.Build();
        context.AddPolicy(policy);
    }
}