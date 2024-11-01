using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class CacheLookupValueCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.CacheLookupValue);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<CacheLookupValueConfig>(context, "cache-lookup-value", out var values))
        {
            return;
        }

        var element = new XElement("cache-lookup-value");
        
        if (!element.AddAttribute(values, nameof(CacheLookupValueConfig.Key), "key"))
        {
            context.ReportError(
                $"{nameof(CacheLookupValueConfig.Key)} is required for cache-lookup-value policy. {node.GetLocation()}");
            return;
        }
        
        if (!element.AddAttribute(values, nameof(CacheLookupValueConfig.VariableName), "variable-name"))
        {
            context.ReportError(
                $"{nameof(CacheLookupValueConfig.VariableName)} is required for cache-lookup-value policy. {node.GetLocation()}");
            return;
        }
        
        element.AddAttribute(values, nameof(CacheLookupValueConfig.CachingType), "caching-type");
        element.AddAttribute(values, nameof(CacheLookupValueConfig.DefaultValue), "default-value");
        
        context.AddPolicy(element);
    }
}