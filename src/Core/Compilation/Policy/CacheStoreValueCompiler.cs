// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class CacheStoreValueCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.CacheStoreValue);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<CacheStoreValueConfig>(context, "cache-store-value", out var values))
        {
            return;
        }

        var element = new XElement("cache-store-value");
        
        if (!element.AddAttribute(values, nameof(CacheStoreValueConfig.Key), "key"))
        {
            context.ReportError(
                $"{nameof(CacheStoreValueConfig.Key)} is required for cache-store-value policy. {node.GetLocation()}");
            return;
        }
        
        if(!element.AddAttribute(values, nameof(CacheStoreValueConfig.Value), "value"))
        {
            context.ReportError(
                $"{nameof(CacheStoreValueConfig.Value)} is required for cache-store-value policy. {node.GetLocation()}");
            return;
        }
        
        if(!element.AddAttribute(values, nameof(CacheStoreValueConfig.Duration), "duration"))
        {
            context.ReportError(
                $"{nameof(CacheStoreValueConfig.Duration)} is required for cache-store-value policy. {node.GetLocation()}");
            return;
        }
        
        element.AddAttribute(values, nameof(CacheStoreValueConfig.CachingType), "caching-type");
        
        context.AddPolicy(element);
    }
}