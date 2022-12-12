using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class ValidateClientCertificatePolicyHandler : MarshallerHandler<ValidateClientCertificatePolicy>
{
    public override void Marshal(Marshaller marshaller, ValidateClientCertificatePolicy element)
    {
        marshaller.Writer.WriteStartElement("validate-client-certificate");

        marshaller.Writer.WriteNullableAttribute("validate-revocation", element.ValidateRevocation);
        marshaller.Writer.WriteNullableAttribute("validate-trust", element.ValidateTrust);
        marshaller.Writer.WriteNullableAttribute("validate-not-before", element.ValidateNotBefore);
        marshaller.Writer.WriteNullableAttribute("validate-not-after", element.ValidateNotAfter);
        marshaller.Writer.WriteNullableAttribute("ignore-error", element.IgnoreError);

        marshaller.Writer.WriteStartElement("identities");
        foreach (var identity in element.Identities)
        {
            marshaller.Writer.WriteStartElement("identity ");
            marshaller.Writer.WriteNullableAttribute("thumbprint", identity.Thumbprint);
            marshaller.Writer.WriteNullableAttribute("serial-number", identity.SerialNumber);
            marshaller.Writer.WriteNullableAttribute("common-name", identity.CommonName);
            marshaller.Writer.WriteNullableAttribute("subject", identity.Subject);
            marshaller.Writer.WriteNullableAttribute("dns-name", identity.DnsName);
            marshaller.Writer.WriteNullableAttribute("issuer-subject", identity.IssuerSubject);
            marshaller.Writer.WriteNullableAttribute("issuer-thumbprint", identity.IssuerThumbprint);
            marshaller.Writer.WriteNullableAttribute("issuer-certificate-id", identity.IssuerCertificateId);

            marshaller.Writer.WriteEndElement();
        }
        marshaller.Writer.WriteEndElement();

        marshaller.Writer.WriteEndElement();
    }
}
