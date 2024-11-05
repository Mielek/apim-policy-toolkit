using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SendRequestCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SendRequest);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<SendRequestConfig>(context, "send-request", out var values))
        {
            return;
        }

        var element = new XElement("send-request");

        if (!element.AddAttribute(values, nameof(SendRequestConfig.ResponseVariableName), "response-variable-name"))
        {
            context.ReportError($"{nameof(SendRequestConfig.ResponseVariableName)}. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(SendRequestConfig.Mode), "mode");
        element.AddAttribute(values, nameof(SendRequestConfig.Timeout), "timeout");
        element.AddAttribute(values, nameof(SendRequestConfig.IgnoreError), "ignore-error");

        if (values.TryGetValue(nameof(SendRequestConfig.Url), out var url))
        {
            element.Add(new XElement("set-url", url.Value!));
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Method), out var method))
        {
            element.Add(new XElement("set-method", method.Value!));
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Headers), out var headers))
        {
            SetHeaderCompiler.HandleHeaders(context, element, headers);
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Body), out var body))
        {
            SetBodyCompiler.HandleBody(context, element, body);
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Authentication), out var authentication))
        {
            HandleAuthentication(context, element, authentication);
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Proxy), out var proxy))
        {
            HandleProxy(context, element, proxy);
        }

        context.AddPolicy(element);
    }

    private void HandleProxy(ICompilationContext context, XElement element, InitializerValue value)
    {
        if (!value.TryGetValues<ProxyConfig>(out var values))
        {
            context.ReportError($"{nameof(ProxyConfig)} initializer. {value.Node.GetLocation()}");
            return;
        }

        var certificateElement = new XElement("proxy");
        if (!certificateElement.AddAttribute(values, nameof(ProxyConfig.Url), "url"))
        {
            context.ReportError($"{nameof(ProxyConfig.Url)}. {value.Node.GetLocation()}");
            return;
        }

        certificateElement.AddAttribute(values, nameof(ProxyConfig.Username), "username");
        certificateElement.AddAttribute(values, nameof(ProxyConfig.Password), "password");
        element.Add(certificateElement);
    }

    private void HandleAuthentication(ICompilationContext context, XElement element, InitializerValue authentication)
    {
        var values = authentication.NamedValues;
        if (values is null)
        {
            context.ReportError(
                $"{nameof(SendRequestConfig.Authentication)} initializer. {authentication.Node.GetLocation()}");
            return;
        }

        switch (authentication.Type)
        {
            case nameof(CertificateAuthenticationConfig):
                HandleCertificateAuthentication(context, element, values, authentication.Node);
                break;
            case nameof(BasicAuthenticationConfig):
                HandleBasicAuthentication(context, element, values, authentication.Node);
                break;
            case nameof(ManagedIdentityAuthenticationConfig):
                HandleManagedIdentityAuthentication(context, element, values, authentication.Node);
                break;
            default:
                context.ReportError(
                    $"{nameof(SendRequestConfig.Authentication)} {authentication.Type}. {authentication.Node.GetLocation()}");
                break;
        }
    }

    private void HandleBasicAuthentication(
        ICompilationContext context,
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        SyntaxNode node)
    {
        var basicElement = new XElement("authentication-basic");
        if (!basicElement.AddAttribute(values, nameof(BasicAuthenticationConfig.Username), "username"))
        {
            context.ReportError($"{nameof(BasicAuthenticationConfig.Username)}. {node.GetLocation()}");
            return;
        }

        if (!basicElement.AddAttribute(values, nameof(BasicAuthenticationConfig.Password), "password"))
        {
            context.ReportError($"{nameof(BasicAuthenticationConfig.Password)}. {node.GetLocation()}");
            return;
        }

        element.Add(basicElement);
    }

    private void HandleCertificateAuthentication(
        ICompilationContext context,
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        SyntaxNode node)
    {
        var certElement = new XElement("authentication-certificate");
        var thumbprint = certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Thumbprint), "thumbprint");
        var certId = certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.CertificateId), "certificate-id");
        var body = certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Body), "body");
        certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Password), "password");

        if (!(thumbprint ^ certId ^ body))
        {
            context.ReportError(
                $"One of {nameof(CertificateAuthenticationConfig.Thumbprint)}, {nameof(CertificateAuthenticationConfig.CertificateId)}, {nameof(CertificateAuthenticationConfig.Body)} must be present. {node.GetLocation()}");
            return;
        }

        element.Add(certElement);
    }

    private void HandleManagedIdentityAuthentication(
        ICompilationContext context,
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        SyntaxNode node)
    {
        var certElement = new XElement("authentication-managed-identity");
        certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.Resource), "resource");
        certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.ClientId), "client-id");
        certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.OutputTokenVariableName),
            "output-token-variable-name");
        certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.IgnoreError), "ignore-error");
        element.Add(certElement);
    }
}