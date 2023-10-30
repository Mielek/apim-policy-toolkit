namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class QuotaPolicyBuilder
    {
        private uint? _renewalPeriod;
        private uint? _calls;
        private uint? _bandwidth;

        [IgnoreBuilderField]
        private ImmutableList<QuotaApi>.Builder? _apis;

        public QuotaPolicyBuilder Api(Action<QuotaApiBuilder> configurator)
        {
            var builder = new QuotaApiBuilder();
            configurator(builder);
            (_apis ??= ImmutableList.CreateBuilder<QuotaApi>()).Add(builder.Build());
            return this;
        }

        public QuotaPolicy Build()
        {
            if (!_renewalPeriod.HasValue) throw new NullReferenceException();

            return new QuotaPolicy(_renewalPeriod.Value, _calls, _bandwidth, _apis?.ToImmutable());
        }
    }

    [GenerateBuilderSetters]
    public partial class QuotaApiBuilder
    {
        private uint? _calls;
        private string? _name;
        private string? _id;

        [IgnoreBuilderField]
        private ImmutableList<QuotaOperation>.Builder? _operations;

        public QuotaApiBuilder Api(Action<QuotaOperationBuilder> configurator)
        {
            var builder = new QuotaOperationBuilder();
            configurator(builder);
            (_operations ??= ImmutableList.CreateBuilder<QuotaOperation>()).Add(builder.Build());
            return this;
        }

        public QuotaApi Build()
        {
            if (!_calls.HasValue) throw new NullReferenceException();
            if (_name == null && _id == null) throw new NullReferenceException();

            return new QuotaApi(_calls.Value, _name, _id, _operations?.ToImmutable());
        }
    }

    [GenerateBuilderSetters]
    public partial class QuotaOperationBuilder
    {
        private uint? _calls;
        private string? _name;
        private string? _id;

        public QuotaOperation Build()
        {
            if (!_calls.HasValue) throw new NullReferenceException();
            if (_name == null && _id == null) throw new NullReferenceException();

            return new QuotaOperation(_calls.Value, _name, _id);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder Quota(Action<QuotaPolicyBuilder> configurator)
        {
            var builder = new QuotaPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}