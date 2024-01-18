using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class CacheStorePolicyBuilder : BaseBuilder<CacheStorePolicyBuilder>
{
    private IExpression<uint>? _duration;
    private bool? _cacheResponse;

    public XElement Build()
    {
        if (_duration == null)
            throw new PolicyValidationException("Duration is required for CacheStore");

        var element = this.CreateElement("cache-store");

        element.Add(_duration.GetXAttribute("duration"));

        if (_cacheResponse != null)
        {
            element.Add(new XAttribute("client-id", _cacheResponse));
        }

        return element;
    }
}