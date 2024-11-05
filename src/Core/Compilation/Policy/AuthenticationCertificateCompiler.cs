using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class AuthenticationCertificateCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.AuthenticationCertificate);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<CertificateAuthenticationConfig>(
                context,
                "authentication-certificate",
                out var values))
        {
            return;
        }

        var certElement = new XElement("authentication-certificate");
        var thumbprint = certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Thumbprint), "thumbprint");
        var certId = certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.CertificateId), "certificate-id");
        var body = certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Body), "body");
        if (!(thumbprint ^ certId ^ body))
        {
            context.ReportError(
                $"One of {nameof(CertificateAuthenticationConfig.Thumbprint)}, {nameof(CertificateAuthenticationConfig.CertificateId)}, {nameof(CertificateAuthenticationConfig.Body)} must be present. {node.GetLocation()}");
            return;
        }

        certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Password), "password");
        context.AddPolicy(certElement);
    }
}