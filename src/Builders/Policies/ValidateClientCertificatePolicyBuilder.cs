namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generators.Attributes;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ValidateClientCertificatePolicyBuilder
    {

        [IgnoreBuilderField]
        private readonly ImmutableList<ValidateClientCertificateIdentity>.Builder _identities = ImmutableList.CreateBuilder<ValidateClientCertificateIdentity>();
        private bool? _validateRevocation;
        private bool? _validateTrust;
        private bool? _validateNotBefore;
        private bool? _validateNotAfter;
        private bool? _ignoreError;

        public ValidateClientCertificatePolicy Build()
        {
            if (_identities.Count == 0) throw new Exception();

            return new ValidateClientCertificatePolicy(
                _identities.ToImmutable(),
                _validateRevocation,
                _validateTrust,
                _validateNotBefore,
                _validateNotAfter,
                _ignoreError);
        }
    }

    [GenerateBuilderSetters]
    public partial class ValidateClientCertificateIdentityBuilder
    {
        private string? _thumbprint;
        private string? _serialNumber;
        private string? _commonName;
        private string? _subject;
        private string? _dnsName;
        private string? _issuerSubject;
        private string? _issuerThumbprint;
        private string? _issuerCertificateId;

        public ValidateClientCertificateIdentity Build()
        {
            return new ValidateClientCertificateIdentity(
                _thumbprint,
                _serialNumber,
                _commonName,
                _subject,
                _dnsName,
                _issuerSubject,
                _issuerThumbprint,
                _issuerCertificateId
            );
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder ValidateClientCertificate(Action<ValidateClientCertificatePolicyBuilder> configurator)
        {
            var builder = new ValidateClientCertificatePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}