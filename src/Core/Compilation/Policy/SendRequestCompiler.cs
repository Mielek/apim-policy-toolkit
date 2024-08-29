using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

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
            HandleHeaders(context, element, headers);
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Body), out var body))
        {
            HandleBody(context, element, body);
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

    private static void HandleHeaders(ICompilationContext context, XElement root, InitializerValue headers)
    {
        foreach (var header in headers.UnnamedValues!)
        {
            if (!header.TryGetValues<HeaderConfig>(out var headerValues))
            {
                context.ReportError($"{nameof(HeaderConfig)}. {header.Node.GetLocation()}");
                continue;
            }

            var headerElement = new XElement("set-header");
            if (!headerElement.AddAttribute(headerValues, nameof(HeaderConfig.Name), "name"))
            {
                continue;
            }

            headerElement.AddAttribute(headerValues, nameof(HeaderConfig.ExistsAction), "exists-action");

            if (headerValues.TryGetValue(nameof(HeaderConfig.Values), out var values) &&
                values.UnnamedValues is not null)
            {
                foreach (var value in values.UnnamedValues)
                {
                    headerElement.Add(new XElement("value", value.Value!));
                }
            }

            root.Add(headerElement);
        }
    }

    private void HandleBody(ICompilationContext context, XElement element, InitializerValue body)
    {
        if (!body.TryGetValues<BodyConfig>(out var config))
        {
            context.ReportError($"{nameof(BodyConfig)}. {body.Node.GetLocation()}");
            return;
        }

        if (!config.TryGetValue(nameof(BodyConfig.Content), out var content))
        {
            context.ReportError($"{nameof(BodyConfig.Content)}. {body.Node.GetLocation()}");
            return;
        }

        var bodyElement = new XElement("set-body", content.Value!);
        bodyElement.AddAttribute(config, nameof(BodyConfig.Template), "template");
        bodyElement.AddAttribute(config, nameof(BodyConfig.XsiNil), "xsi-nil");
        bodyElement.AddAttribute(config, nameof(BodyConfig.ParseDate), "parse-date");
        element.Add(bodyElement);
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
        certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Thumbprint), "thumbprint");
        certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.CertificateId), "certificate-id");
        certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Body), "body");
        certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Password), "password");
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