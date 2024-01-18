using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class CacheStoreValuePolicyBuilder : BaseBuilder<CacheRemoveValuePolicyBuilder>
{
    public enum CachingTypeEnum { Internal, External, PreferExternal }

    private string? _key;
    private IExpression<string>? _value;
    private IExpression<uint>? _duration;
    private CachingTypeEnum? _cachingType;

    public XElement Build()
    {
        if (_key == null) throw new PolicyValidationException("Key is required for CacheStoreValue");
        if (_value == null) throw new PolicyValidationException("Value is required for CacheStoreValue");
        if (_duration == null) throw new PolicyValidationException("Duration is required for CacheStoreValue");

        var element = this.CreateElement("cache-store-value");

        element.Add(new XAttribute("key", _key));
        element.Add(_value.GetXAttribute("value"));
        element.Add(_duration.GetXAttribute("duration"));

        if (_cachingType != null)
        {
            element.Add(new XAttribute("caching-type", TranslateCachingType(_cachingType)));
        }

        return element;
    }

    private string TranslateCachingType(CachingTypeEnum? cachingType)
    {
        return cachingType switch
        {
            CachingTypeEnum.Internal => "internal",
            CachingTypeEnum.External => "external",
            CachingTypeEnum.PreferExternal => "prefer-external",
            _ => throw new PolicyValidationException("Unknown caching type for CacheStoreValue"),
        };
    }
}