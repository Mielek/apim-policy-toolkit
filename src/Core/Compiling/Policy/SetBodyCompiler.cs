// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

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
                        context.ReportError($"Not liquid. {node.GetLocation()}");
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
                        context.ReportError($"Not bank or null. {node.GetLocation()}");
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

    public static void HandleBody(ICompilationContext context, XElement element, InitializerValue body)
    {
        if (!body.TryGetValues<BodyConfig>(out var config))
        {
            context.ReportError($"{nameof(BodyConfig)}. {body.Node.GetLocation()}");
            return;
        }

        if (!config.TryGetValue(nameof(BodyConfig.Content), out var content))
        {
            context.ReportError($"{nameof(BodyConfig.Content)}. {body.Node.GetLocation()}");
            return;
        }

        var bodyElement = new XElement("set-body", content.Value!);
        bodyElement.AddAttribute(config, nameof(BodyConfig.Template), "template");
        bodyElement.AddAttribute(config, nameof(BodyConfig.XsiNil), "xsi-nil");
        bodyElement.AddAttribute(config, nameof(BodyConfig.ParseDate), "parse-date");
        element.Add(bodyElement);
    }
}