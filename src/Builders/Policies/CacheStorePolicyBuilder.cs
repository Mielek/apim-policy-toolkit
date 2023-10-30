namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class CacheStorePolicyBuilder
    {
        private IExpression<uint>? _duration;
        private bool? _cacheResponse;

        public CacheStorePolicy Build()
        {
            if (_duration == null) throw new NullReferenceException();

            return new CacheStorePolicy(_duration, _cacheResponse);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder CacheStore(Action<CacheStorePolicyBuilder> configurator)
        {
            var builder = new CacheStorePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}