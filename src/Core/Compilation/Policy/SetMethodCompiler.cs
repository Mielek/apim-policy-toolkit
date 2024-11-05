using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetMethodCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetMethod);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for set-method policy. {node.GetLocation()}");
            return;
        }

        var value = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("set-method", value));
    }
}