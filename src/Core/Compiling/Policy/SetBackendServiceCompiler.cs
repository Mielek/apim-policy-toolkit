// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class SetBackendServiceCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetBackendService);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<SetBackendServiceConfig>(context, "set-backend-service", out var values))
        {
            return;
        }

        var element = new XElement("set-backend-service");

        var baseUrlDefined = element.AddAttribute(values, nameof(SetBackendServiceConfig.BaseUrl), "base-url");
        var backendIdDefined = element.AddAttribute(values, nameof(SetBackendServiceConfig.BackendId), "backend-id");
        if (!(baseUrlDefined ^ backendIdDefined))
        {
            context.ReportError(
                $"You need to specify either base-url or backend-id but not both. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfResolveCondition), "sf-resolve-condition");
        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfServiceInstanceName), "sf-service-instance-name");
        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfPartitionKey), "sf-partition-key");
        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfListenerName), "sf-listener-name");

        context.AddPolicy(element);
    }
}