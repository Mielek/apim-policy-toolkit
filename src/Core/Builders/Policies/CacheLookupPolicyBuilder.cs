namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class CacheLookupPolicyBuilder
    {
        private bool? _varyByDeveloper;
        private bool? _varyByDeveloperGroup;
        private CacheLookupCachingType? _catchingType;
        private CacheLookupDownstreamCachingType? _downstreamCachingType;
        private bool? _mustRevalidate;
        private IExpression<bool>? _allowPrivateResponseCaching;
        private ImmutableList<string>.Builder? _varyByHeaders;
        private ImmutableList<string>.Builder? _varyByQueryParameters;

        public XElement Build()
        {
            if (_varyByDeveloper == null) throw new NullReferenceException();
            if (_varyByDeveloperGroup == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();


            children.Add(new XAttribute("vary-by-developer", _varyByDeveloper));
            children.Add(new XAttribute("vary-by-developer-group", _varyByDeveloperGroup));

            if (_catchingType != null)
            {
                children.Add(new XAttribute("cache-type", TranslateCachingType(_catchingType)));
            }
            if (_downstreamCachingType != null)
            {
                children.Add(new XAttribute("downstream-caching-type", TranslateDownstreamCachingType(_downstreamCachingType)));
            }
            if (_mustRevalidate != null)
            {
                children.Add(new XAttribute("must-revalidate", _mustRevalidate));
            }
            if (_allowPrivateResponseCaching != null)
            {
                children.Add(_allowPrivateResponseCaching.GetXAttribute("allow-private-response-caching"));
            }

            if (_varyByHeaders != null)
            {
                foreach (var varyByHeader in _varyByHeaders)
                {
                    children.Add(new XElement("vary-by-header", varyByHeader));
                }
            }

            if (_varyByQueryParameters != null)
            {
                foreach (var varyByQueryParam in _varyByQueryParameters)
                {
                    children.Add(new XElement("vary-by-query-parameter", varyByQueryParam));
                }
            }

            return new XElement("cache-lookup", children.ToArray());
        }

        private static string TranslateCachingType(CacheLookupCachingType? type) => type switch
        {
            CacheLookupCachingType.PreferExternal => "prefer-external",
            CacheLookupCachingType.External => "external",
            CacheLookupCachingType.Internal => "internal",
            _ => throw new Exception(),
        };

        private static string TranslateDownstreamCachingType(CacheLookupDownstreamCachingType? type) => type switch
        {
            CacheLookupDownstreamCachingType.None => "none",
            CacheLookupDownstreamCachingType.Private => "private",
            CacheLookupDownstreamCachingType.Public => "public",
            _ => throw new Exception(),
        };

        public enum CacheLookupCachingType { PreferExternal, External, Internal }
        public enum CacheLookupDownstreamCachingType { None, Private, Public }
    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder CacheLookup(Action<CacheLookupPolicyBuilder> configurator)
        {
            var builder = new CacheLookupPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}