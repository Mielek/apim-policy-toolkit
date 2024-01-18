using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class CacheStoreValuePolicyBuilder
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
            _ => throw new PolicyValidationException("Unknown caching type for CacheStoreValue"),
        };
    }
}