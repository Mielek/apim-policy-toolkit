using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetVariableCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetVariable);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 2)
        {
            context.ReportError($"Wrong argument count for set-variable policy. {node.GetLocation()}");
            return;
        }
        var name = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        var value = node.ArgumentList.Arguments[1].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("set-variable", new XAttribute("name", name), new XAttribute("value", value)));
    }
}