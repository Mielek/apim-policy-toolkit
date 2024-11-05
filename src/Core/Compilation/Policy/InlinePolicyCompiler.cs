using System.Xml;
using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Serialization;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

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

        if (expression is not LiteralExpressionSyntax literal)
        {
            context.ReportError($"Inline policy must be a string literal. {node.GetLocation()}");
            return;
        }

        XElement xml = CreateRazorFromString(literal);
        context.AddPolicy(xml);
    }

    private static XElement CreateRazorFromString(LiteralExpressionSyntax literal)
    {
        var cleanXml = RazorCodeFormatter.ToCleanXml(literal.Token.ValueText, out var markerToCode);
        var xml = XElement.Parse(cleanXml);

        foreach (XElement element in xml.DescendantsAndSelf())
        {
            if (element.HasAttributes)
            {
                foreach (var a in element.Attributes())
                {
                    if (markerToCode.TryGetValue(a.Value, out var attributeCode))
                    {
                        a.Value = attributeCode;
                    }
                }
            }

            if (markerToCode.TryGetValue(element.Value, out var valueCode))
            {
                element.Value = valueCode;
            }
        }

        return xml;
    }
}