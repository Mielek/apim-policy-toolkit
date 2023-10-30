namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

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

        public CacheLookupPolicy Build()
        {
            if (_varyByDeveloper == null) throw new NullReferenceException();
            if (_varyByDeveloperGroup == null) throw new NullReferenceException();

            return new CacheLookupPolicy(
                _varyByDeveloper.Value,
                _varyByDeveloperGroup.Value,
                _catchingType, _downstreamCachingType,
                _mustRevalidate,
                _allowPrivateResponseCaching,
                _varyByHeaders?.ToImmutable(),
                _varyByQueryParameters?.ToImmutable());
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

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