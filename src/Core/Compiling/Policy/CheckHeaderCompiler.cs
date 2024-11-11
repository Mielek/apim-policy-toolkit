// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class CheckHeaderCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.CheckHeader);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<CheckHeaderConfig>(context, "check-header", out var values))
        {
            return;
        }

        var element = new XElement("check-header");

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.Name), "name"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.Name)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.FailCheckHttpCode), "failed-check-httpcode"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.FailCheckHttpCode)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.FailCheckErrorMessage),
                "failed-check-error-message"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.FailCheckErrorMessage)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.IgnoreCase), "ignore-case"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.IgnoreCase)}. {node.GetLocation()}");
            return;
        }

        if (!values.TryGetValue(nameof(CheckHeaderConfig.Values), out var headerValues))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.Values)}. {node.GetLocation()}");
            return;
        }

        var elements = (headerValues.UnnamedValues ?? [])
            .Select(origin => new XElement("value", origin.Value!))
            .ToArray<object>();
        if (elements.Length == 0)
        {
            context.ReportError(
                $"{nameof(CheckHeaderConfig.Values)} must have at least one value. {node.GetLocation()}");
            return;
        }

        element.Add(elements);

        context.AddPolicy(element);
    }
}