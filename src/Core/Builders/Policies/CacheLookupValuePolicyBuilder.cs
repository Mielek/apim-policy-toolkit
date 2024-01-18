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
public partial class CacheLookupValuePolicyBuilder
{
    public enum CachingTypeEnum { Internal, External, PreferExternal }

    private string? _variableName;
    private CachingTypeEnum? _catchingType;
    private IExpression<string>? _key;
    private IExpression<string>? _defaultValue;

    public XElement Build()
    {
        if (_variableName == null) throw new PolicyValidationException("Variable name is required for CacheLookupValue");

        var children = ImmutableArray.CreateBuilder<XObject>();
        children.Add(new XAttribute("variable-name", _variableName));
        if (_catchingType != null)
        {
            children.Add(new XAttribute("caching-type", TranslateCachingType(_catchingType)));
        }
        if (_key != null)
        {
            children.Add(_key.GetXAttribute("key"));
        }
        if (_defaultValue != null)
        {
            children.Add(_defaultValue.GetXAttribute("default-value"));
        }

        return new XElement("cache-lookup-value", children.ToArray());
    }

    private string TranslateCachingType(CachingTypeEnum? cachingType)
    {
        return cachingType switch
        {
            CachingTypeEnum.Internal => "internal",
            CachingTypeEnum.External => "external",
            CachingTypeEnum.PreferExternal => "prefer-external",
            _ => throw new PolicyValidationException("Unknown caching type for CacheLookupValue"),
        };
    }
}
