using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetBodyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetBody);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count is > 2 or 0)
        {
            context.ReportError($"Wrong argument count for set-body policy. {node.GetLocation()}");
            return;
        }

        var value = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        var element = new XElement("set-body", value);
        if (node.ArgumentList.Arguments.Count == 2)
        {
            var contentType = node.ArgumentList.Arguments[1].Expression.ProcessExpression(context);
            if (contentType is { Type: nameof(SetBodyConfig), NamedValues: not null })
            {
                if (contentType.NamedValues.TryGetValue(nameof(SetBodyConfig.Template), out var template))
                {
                    if (template.Value != "liquid")
                    {
                        context.ReportError($"TODO. {node.GetLocation()}");
                    }
                    else
                    {
                        element.Add(new XAttribute("template", template.Value));
                    }
                }

                if (contentType.NamedValues.TryGetValue(nameof(SetBodyConfig.XsiNil), out var xsiNil))
                {
                    if (xsiNil.Value != "blank" && xsiNil.Value != "null")
                    {
                        context.ReportError($"TODO. {node.GetLocation()}");
                    }
                    else
                    {
                        element.Add(new XAttribute("xsi-nil", xsiNil.Value));
                    }
                }

                if (contentType.NamedValues.TryGetValue(nameof(SetBodyConfig.ParseDate), out var parseDate))
                {
                    element.Add(new XAttribute("parse-date", parseDate.Value!));
                }
            }
        }

        context.AddPolicy(element);
    }
}