using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class AuthenticationCertificatePolicyHandler : MarshallerHandler<AuthenticationCertificatePolicy>
{
    public override void Marshal(Marshaller marshaller, AuthenticationCertificatePolicy element)
    {
        marshaller.Writer.WriteStartElement("authentication-certificate");

        marshaller.Writer.WriteNullableAttribute("thumbprint", element.Thumbprint);
        marshaller.Writer.WriteNullableAttribute("certificate-id", element.CertificateId);
        marshaller.Writer.WriteNullableExpressionAsAttribute("body", element.Body);
        marshaller.Writer.WriteNullableAttribute("password", element.Body);

        marshaller.Writer.WriteEndElement();
    }
}