using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using static Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetHeaderPolicyBuilder;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetHeaderCompiler : IMethodPolicyHandler
{
    public static SetHeaderCompiler AppendCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.AppendHeader), ExistsActionType.Append);

    public static SetHeaderCompiler SetCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.SetHeader), ExistsActionType.Override);

    public static SetHeaderCompiler SetIfNotExistCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.SetHeaderIfNotExist), ExistsActionType.Skip);

    public static SetHeaderCompiler RemoveCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.RemoveHeader), ExistsActionType.Delete);

    readonly ExistsActionType _type;

    private SetHeaderCompiler(string methodName, ExistsActionType type)
    {
        MethodName = methodName;
        _type = type;
    }

    public string MethodName { get; }

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (_type != ExistsActionType.Delete && arguments.Count < 2 ||
            _type == ExistsActionType.Delete && arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for set-header policy. {node.GetLocation()}");
            context.AddPolicy(new XComment("Issue: set-header"));
            return;
        }

        var headerName = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        var builder = new SetHeaderPolicyBuilder()
            .Name(headerName)
            .ExistsAction(_type);

        for (int i = 1; i < arguments.Count; i++)
        {
            builder.Value(arguments[i].Expression.ProcessParameter(context));
        }

        var policy = builder.Build();
        context.AddPolicy(policy);
    }
}