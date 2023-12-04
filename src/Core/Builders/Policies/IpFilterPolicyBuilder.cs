namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;


    [GenerateBuilderSetters]
    public partial class IpFilterPolicyBuilder
    {
        public enum IpFilterAction { Allow, Forbid }

        public interface IIpFilterValue { };
        public sealed record IpFilterAddress(string Ip) : IIpFilterValue;
        public sealed record IpFilterAddressRange(string FromIp, string ToIp) : IIpFilterValue;

        private IpFilterAction? _action;

        [IgnoreBuilderField]
        private readonly ImmutableList<IIpFilterValue>.Builder _values = ImmutableList.CreateBuilder<IIpFilterValue>();

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
            if (_action == null) throw new NullReferenceException();
            if (_values.Count == 0) throw new Exception();

            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(new XAttribute("action", TranslateAction(_action)));

            foreach (var ipFilterValue in _values)
            {
                switch (ipFilterValue)
                {
                    case IpFilterAddress address:
                        children.Add(new XElement("address", address.Ip));
                        break;
                    case IpFilterAddressRange range:
                        children.Add(new XElement("address-range",
                            new XAttribute("from", range.FromIp),
                            new XAttribute("to", range.ToIp)
                        ));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return new XElement("ip-filter", children.ToArray());
        }

        private string TranslateAction(IpFilterAction? action) => action switch
        {
            IpFilterAction.Allow => "allow",
            IpFilterAction.Forbid => "forbid",
            _ => throw new NotImplementedException(),
        };

    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder IpFilter(Action<IpFilterPolicyBuilder> configurator)
        {
            var builder = new IpFilterPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}