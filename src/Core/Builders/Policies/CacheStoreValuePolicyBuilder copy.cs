namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class CacheStoreValuePolicyBuilder
    {
        public enum CachingTypeEnum { Internal, External, PreferExternal }

        private string? _key;
        private IExpression<string>? _value;
        private IExpression<uint>? _duration;
        private CachingTypeEnum? _cachingType;

        public XElement Build()
        {
            if (_key == null) throw new NullReferenceException();
            if (_value == null) throw new NullReferenceException();
            if (_duration == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(new XAttribute("key", _key));
            children.Add(_value.GetXAttribute("value"));
            children.Add(_duration.GetXAttribute("duration"));

            if (_cachingType != null)
            {
                children.Add(new XAttribute("caching-type", TranslateCachingType(_cachingType)));
            }

            return new XElement("cache-store-value", children.ToArray());
        }

        private string TranslateCachingType(CachingTypeEnum? cachingType)
        {
            return cachingType switch
            {
                CachingTypeEnum.Internal => "internal",
                CachingTypeEnum.External => "external",
                CachingTypeEnum.PreferExternal => "prefer-external",
                _ => throw new NotImplementedException(),
            };
        }
    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder CacheStoreValue(Action<CacheStoreValuePolicyBuilder> configurator)
        {
            var builder = new CacheStoreValuePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}