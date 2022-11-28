namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class IpFilterPolicyBuilder
    {
        IpFilterAction? _action;

        [IgnoreBuilderField]
        readonly ImmutableList<IIpFilterValue>.Builder _values = ImmutableList.CreateBuilder<IIpFilterValue>();

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

        public IpFilterPolicy Build()
        {
            if (_action == null) throw new NullReferenceException();
            if (_values.Count == 0) throw new Exception();

            return new IpFilterPolicy(_action.Value, _values.ToImmutable());
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

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