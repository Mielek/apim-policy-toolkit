namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class CacheRemoveValuePolicyBuilder
    {
        public enum CachingTypeEnum { Internal, External, PreferExternal }

        private IExpression<string>? _key;
        private CachingTypeEnum? _cachingType;

        public XElement Build()
        {
            if (_key == null) throw new NullReferenceException();
           
            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(_key.GetXAttribute("key"));

            if (_cachingType != null)
            {
                children.Add(new XAttribute("caching-type", TranslateCachingType(_cachingType)));
            }

            return new XElement("cache-remove-value", children.ToArray());
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
        public PolicySectionBuilder CacheRemoveValue(Action<CacheStoreValuePolicyBuilder> configurator)
        {
            var builder = new CacheStoreValuePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}