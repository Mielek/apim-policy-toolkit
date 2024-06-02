using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetBodyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetBody);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count < 2)
        {
            context.ReportError("");
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