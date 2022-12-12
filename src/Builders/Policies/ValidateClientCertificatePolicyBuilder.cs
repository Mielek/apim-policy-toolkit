namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ValidateClientCertificatePolicyBuilder
    {

        [IgnoreBuilderField]
        readonly ImmutableList<ValidateClientCertificateIdentity>.Builder _identities = ImmutableList.CreateBuilder<ValidateClientCertificateIdentity>();
        bool? _validateRevocation;
        bool? _validateTrust;
        bool? _validateNotBefore;
        bool? _validateNotAfter;
        bool? _ignoreError;

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
        string? _thumbprint;
        string? _serialNumber;
        string? _commonName;
        string? _subject;
        string? _dnsName;
        string? _issuerSubject;
        string? _issuerThumbprint;
        string? _issuerCertificateId;

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
            this._sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}