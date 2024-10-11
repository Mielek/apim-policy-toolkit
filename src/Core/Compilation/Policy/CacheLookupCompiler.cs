using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class CacheLookupCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.CacheLookup);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<CacheLookupConfig>(context, "check-header", out var values))
        {
            return;
        }

        var element = new XElement("cache-lookup");

        if (!element.AddAttribute(values, nameof(CacheLookupConfig.VaryByDeveloper), "vary-by-developer"))
        {
            context.ReportError(
                $"{nameof(CacheLookupConfig.VaryByDeveloper)} is required for cache-lookup policy. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(CacheLookupConfig.VaryByDeveloperGroups), "vary-by-developer-groups"))
        {
            context.ReportError(
                $"{nameof(CacheLookupConfig.VaryByDeveloperGroups)} is required for cache-lookup policy. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(CacheLookupConfig.CachingType), "caching-type");
        element.AddAttribute(values, nameof(CacheLookupConfig.DownstreamCachingType), "downstream-caching-type");
        element.AddAttribute(values, nameof(CacheLookupConfig.MustRevalidate), "must-revalidate");
        element.AddAttribute(values, nameof(CacheLookupConfig.AllowPrivateResponseCaching), "allow-private-response-caching");

        if (values.TryGetValue(nameof(CacheLookupConfig.VaryByHeaders), out var headers) &&
            headers.UnnamedValues is not null)
        {
            foreach (var value in headers.UnnamedValues)
            {
                element.Add(new XElement("vary-by-header", value.Value!));
            }
        }

        if (values.TryGetValue(nameof(CacheLookupConfig.VaryByQueryParameters), out var queryParams) &&
            queryParams.UnnamedValues is not null)
        {
            foreach (var value in queryParams.UnnamedValues)
            {
                element.Add(new XElement("vary-by-query-parameter", value.Value!));
            }
        }

        context.AddPolicy(element);
    }
}