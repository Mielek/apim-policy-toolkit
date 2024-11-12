// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class CacheRemoveValueCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.CacheRemoveValue);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<CacheRemoveValueConfig>(context, "cache-remove-value", out var values))
        {
            return;
        }

        var element = new XElement("cache-remove-value");
        
        if (!element.AddAttribute(values, nameof(CacheRemoveValueConfig.Key), "key"))
        {
            context.ReportError(
                $"{nameof(CacheRemoveValueConfig.Key)} is required for cache-remove-value policy. {node.GetLocation()}");
            return;
        }
        
        element.AddAttribute(values, nameof(CacheRemoveValueConfig.CachingType), "caching-type");
        
        context.AddPolicy(element);
    }
}