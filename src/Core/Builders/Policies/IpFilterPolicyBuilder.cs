using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class IpFilterPolicyBuilder : BaseBuilder<IpFilterPolicyBuilder>
{
    public enum IpFilterAction { Allow, Forbid }

    public interface IIpFilterValue { };
    public sealed record IpFilterAddress(string Ip) : IIpFilterValue;
    public sealed record IpFilterAddressRange(string FromIp, string ToIp) : IIpFilterValue;

    private IpFilterAction? _action;

    [IgnoreBuilderField]
    private ImmutableList<IIpFilterValue>.Builder _values = ImmutableList.CreateBuilder<IIpFilterValue>();

    public IpFilterPolicyBuilder Address(string address)
    {
        _values.Add(new IpFilterAddress(address));
        return this;
    }

    public IpFilterPolicyBuilder AddressRange(string from, string to)
    {
        _values.Add(new IpFilterAddressRange(from, to));
        return this;
    }

    public XElement Build()
    {
        if (_action == null) throw new PolicyValidationException("Action is required for IpFilter");
        if (_values == null || _values.Count == 0) throw new PolicyValidationException("At least one Address or AddressRange is required for IpFilter");

        var element = CreateElement("ip-filter");

        element.Add(new XAttribute("action", TranslateAction(_action)));

        foreach (var ipFilterValue in _values)
        {
            switch (ipFilterValue)
            {
                case IpFilterAddress address:
                    element.Add(new XElement("address", address.Ip));
                    break;
                case IpFilterAddressRange range:
                    element.Add(new XElement("address-range",
                        new XAttribute("from", range.FromIp),
                        new XAttribute("to", range.ToIp)
                    ));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return element;
    }

    private string TranslateAction(IpFilterAction? action) => action switch
    {
        IpFilterAction.Allow => "allow",
        IpFilterAction.Forbid => "forbid",
        _ => throw new NotImplementedException(),
    };

}