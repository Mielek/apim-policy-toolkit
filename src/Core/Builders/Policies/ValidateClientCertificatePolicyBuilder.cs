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
public partial class ValidateClientCertificatePolicyBuilder : BaseBuilder<ValidateClientCertificatePolicyBuilder>
{

    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _identities;
    private bool? _validateRevocation;
    private bool? _validateTrust;
    private bool? _validateNotBefore;
    private bool? _validateNotAfter;
    private bool? _ignoreError;


    public ValidateClientCertificatePolicyBuilder RequiredClaim(Action<ValidateClientCertificateIdentityBuilder> configurator)
    {
        var builder = new ValidateClientCertificateIdentityBuilder();
        configurator(builder);
        (_identities ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public XElement Build()
    {
        if (_identities == null || _identities.Count == 0) throw new PolicyValidationException("At least one identity is required for ValidateClientCertificate");

        var element = CreateElement("validate-client-certificate");
        if (_validateRevocation != null)
        {
            element.Add(new XAttribute("validate-revocation", _validateRevocation));
        }
        if (_validateTrust != null)
        {
            element.Add(new XAttribute("validate-trust", _validateTrust));
        }
        if (_validateNotBefore != null)
        {
            element.Add(new XAttribute("validate-not-before", _validateNotBefore));
        }
        if (_validateNotAfter != null)
        {
            element.Add(new XAttribute("validate-not-after", _validateNotAfter));
        }
        if (_ignoreError != null)
        {
            element.Add(new XAttribute("ignore-error", _ignoreError));
        }

        element.Add(new XElement("identities", _identities.ToArray()));

        return element;
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

    public XElement Build()
    {
        var children = ImmutableArray.CreateBuilder<object>();

        if (_thumbprint != null)
        {
            children.Add(new XAttribute("thumbprint", _thumbprint));
        }
        if (_serialNumber != null)
        {
            children.Add(new XAttribute("serial-number", _serialNumber));
        }
        if (_commonName != null)
        {
            children.Add(new XAttribute("common-name", _commonName));
        }
        if (_subject != null)
        {
            children.Add(new XAttribute("subject", _subject));
        }
        if (_dnsName != null)
        {
            children.Add(new XAttribute("dns-name", _dnsName));
        }
        if (_issuerSubject != null)
        {
            children.Add(new XAttribute("issuer-subject", _issuerSubject));
        }
        if (_issuerThumbprint != null)
        {
            children.Add(new XAttribute("issuer-thumbprint", _issuerThumbprint));
        }
        if (_issuerCertificateId != null)
        {
            children.Add(new XAttribute("issuer-certificate-id", _issuerCertificateId));
        }

        return new XElement("identity", children.ToArray());
    }
}