namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class CacheStorePolicyBuilder
    {
        private IExpression<uint>? _duration;
        private bool? _cacheResponse;

        public XElement Build()
        {
            if (_duration == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            
            children.Add(_duration.GetXAttribute("duration"));

            if(_cacheResponse != null)
            {
                children.Add(new XAttribute("client-id", _cacheResponse));
            }

            return new XElement("cache-store", children.ToArray());
        }
    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

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