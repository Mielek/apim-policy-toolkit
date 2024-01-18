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
public partial class CacheRemoveValuePolicyBuilder
{
    public enum CachingTypeEnum { Internal, External, PreferExternal }

    private IExpression<string>? _key;
    private CachingTypeEnum? _cachingType;

    public XElement Build()
    {
        if (_key == null) throw new PolicyValidationException("Key is required for CacheRemoveValue");

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
            _ => throw new PolicyValidationException("Unknown caching type for CacheRemoveValue"),
        };
    }
}