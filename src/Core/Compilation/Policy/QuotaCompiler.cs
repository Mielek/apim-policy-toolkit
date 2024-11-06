// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class QuotaCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.Quota);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<QuotaConfig>(context, "quota", out var values))
        {
            return;
        }

        var element = new XElement("quota");

        var isCallsAdded = element.AddAttribute(values, nameof(QuotaConfig.Calls), "calls");
        var isBandwidthAdded = element.AddAttribute(values, nameof(QuotaConfig.Bandwidth), "bandwidth");

        if (!isCallsAdded && !isBandwidthAdded)
        {
            context.ReportError(
                $"{nameof(QuotaConfig.Calls)} or {nameof(QuotaConfig.Bandwidth)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(QuotaConfig.RenewalPeriod), "renewal-period"))
        {
            context.ReportError($"{nameof(QuotaConfig.RenewalPeriod)}. {node.GetLocation()}");
            return;
        }

        if (values.TryGetValue(nameof(QuotaConfig.Apis), out var apis))
        {
            foreach (var api in apis.UnnamedValues!)
            {
                if (api.Type != nameof(ApiQuota))
                {
                    context.ReportError($"Api must be of type {nameof(ApiQuota)}. {api.Node.GetLocation()}");
                    continue;
                }

                if (!Handle(context, "api", api, out var apiElement))
                {
                    continue;
                }

                element.Add(apiElement);

                if (api.NamedValues!.TryGetValue(nameof(ApiQuota.Operations), out var operations))
                {
                    foreach (var operation in operations.UnnamedValues!)
                    {
                        if (operation.Type != nameof(OperationQuota))
                        {
                            context.ReportError(
                                $"Operation must be of type {nameof(OperationQuota)}. {operation.Node.GetLocation()}");
                            continue;
                        }

                        if (Handle(context, "operation", operation, out var operationElement))
                        {
                            apiElement.Add(operationElement);
                        }
                    }
                }
            }
        }

        context.AddPolicy(element);
    }

    private bool Handle(ICompilationContext context, string name, InitializerValue value, out XElement element)
    {
        element = new XElement(name);
        var values = value.NamedValues!;

        var isNameAdded = element.AddAttribute(values, nameof(EntityQuotaConfig.Name), "name");
        var isIdAdded = element.AddAttribute(values, nameof(EntityQuotaConfig.Id), "id");

        if (!isNameAdded && !isIdAdded)
        {
            context.ReportError(
                $"{nameof(EntityQuotaConfig.Name)} && {nameof(EntityQuotaConfig.Id)}. {value.Node.GetLocation()}");
            return false;
        }

        var isCallsAdded = element.AddAttribute(values, nameof(QuotaConfig.Calls), "calls");
        var isBandwidthAdded = element.AddAttribute(values, nameof(QuotaConfig.Bandwidth), "bandwidth");

        if (!isCallsAdded && !isBandwidthAdded)
        {
            context.ReportError(
                $"{nameof(QuotaConfig.Calls)} or {nameof(QuotaConfig.Bandwidth)}. {value.Node.GetLocation()}");
            return false;
        }

        return true;
    }
}