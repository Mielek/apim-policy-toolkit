using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class CacheLookupPolicyBuilder : BaseBuilder<CacheLookupPolicyBuilder>
{
    public enum CacheLookupCachingType { PreferExternal, External, Internal }

    public enum CacheLookupDownstreamCachingType { None, Private, Public }

    private readonly bool? _varyByDeveloper;
    private readonly bool? _varyByDeveloperGroup;
    private readonly CacheLookupCachingType? _catchingType;
    private readonly CacheLookupDownstreamCachingType? _downstreamCachingType;
    private readonly bool? _mustRevalidate;
    private readonly IExpression<bool>? _allowPrivateResponseCaching;
    private readonly ImmutableList<string>.Builder? _varyByHeaders;
    private readonly ImmutableList<string>.Builder? _varyByQueryParameters;

    public XElement Build()
    {
        if (_varyByDeveloper == null)
            throw new PolicyValidationException("Vary by developer is required for CacheLookup");

        if (_varyByDeveloperGroup == null)
            throw new PolicyValidationException("Vary by developer group is required for CacheLookup");

        var element = this.CreateElement("cache-lookup");

        element.Add(new XAttribute("vary-by-developer", _varyByDeveloper));
        element.Add(new XAttribute("vary-by-developer-group", _varyByDeveloperGroup));

        if (_catchingType != null)
        {
            element.Add(new XAttribute("cache-type", TranslateCachingType(_catchingType)));
        }

        if (_downstreamCachingType != null)
        {
            element.Add(new XAttribute("downstream-caching-type",
                TranslateDownstreamCachingType(_downstreamCachingType)));
        }

        if (_mustRevalidate != null)
        {
            element.Add(new XAttribute("must-revalidate", _mustRevalidate));
        }

        if (_allowPrivateResponseCaching != null)
        {
            element.Add(_allowPrivateResponseCaching.GetXAttribute("allow-private-response-caching"));
        }

        if (_varyByHeaders != null)
        {
            foreach (var varyByHeader in _varyByHeaders)
            {
                element.Add(new XElement("vary-by-header", varyByHeader));
            }
        }

        if (_varyByQueryParameters != null)
        {
            foreach (var varyByQueryParam in _varyByQueryParameters)
            {
                element.Add(new XElement("vary-by-query-parameter", varyByQueryParam));
            }
        }

        return element;
    }

    private static string TranslateCachingType(CacheLookupCachingType? type) => type switch
    {
        CacheLookupCachingType.PreferExternal => "prefer-external",
        CacheLookupCachingType.External => "external",
        CacheLookupCachingType.Internal => "internal",
        _ => throw new PolicyValidationException("Unknown caching type for CacheLookup"),
    };

    private static string TranslateDownstreamCachingType(CacheLookupDownstreamCachingType? type) => type switch
    {
        CacheLookupDownstreamCachingType.None => "none",
        CacheLookupDownstreamCachingType.Private => "private",
        CacheLookupDownstreamCachingType.Public => "public",
        _ => throw new PolicyValidationException("Unknown downstream caching type for CacheLookup"),
    };
}