using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

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