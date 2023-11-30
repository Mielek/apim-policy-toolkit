namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class CacheStorePolicyBuilder
    {
        private IExpression<uint>? _duration;
        private bool? _cacheResponse;

        public XElement Build()
        {
            if (_duration == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            
            children.Add(new XAttribute("duration", _duration.GetXText()));

            if(_cacheResponse != null)
            {
                children.Add(new XAttribute("client-id", _cacheResponse));
            }

            return new XElement("cache-store", children.ToArray());
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