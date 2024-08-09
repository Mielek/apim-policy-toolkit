using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class InlinePolicyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.InlinePolicy);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for inline policy. {node.GetLocation()}");
            return;
        }

        var expression = node.ArgumentList.Arguments[0].Expression;

        if(expression is not LiteralExpressionSyntax literal)
        {
            context.ReportError($"Inline policy must be a string literal. {node.GetLocation()}");
            return;
        }
        
        context.AddPolicy(XElement.Parse(literal.Token.ValueText));
    }
}