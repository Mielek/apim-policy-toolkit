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
public partial class CacheLookupValuePolicyBuilder : BaseBuilder<CacheLookupValuePolicyBuilder>
{
    public enum CachingTypeEnum { Internal, External, PreferExternal }

    private string? _variableName;
    private CachingTypeEnum? _catchingType;
    private IExpression<string>? _key;
    private IExpression<string>? _defaultValue;

    public XElement Build()
    {
        if (_variableName == null)
            throw new PolicyValidationException("Variable name is required for CacheLookupValue");

        var element = CreateElement("cache-lookup-value");
        element.Add(new XAttribute("variable-name", _variableName));
        if (_catchingType != null)
        {
            element.Add(new XAttribute("caching-type", TranslateCachingType(_catchingType)));
        }
        if (_key != null)
        {
            element.Add(_key.GetXAttribute("key"));
        }
        if (_defaultValue != null)
        {
            element.Add(_defaultValue.GetXAttribute("default-value"));
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
            _ => throw new PolicyValidationException("Unknown caching type for CacheLookupValue"),
        };
    }
}