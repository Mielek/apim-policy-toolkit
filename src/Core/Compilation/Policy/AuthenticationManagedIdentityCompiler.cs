using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class AuthenticationManagedIdentityCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.AuthenticationManagedIdentity);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ManagedIdentityAuthenticationConfig>(
                context,
                "authentication-managed-identity",
                out var values))
        {
            return;
        }

        var element = new XElement("authentication-managed-identity");

        if (!element.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.Resource), "resource"))
        {
            context.ReportError($"{nameof(ManagedIdentityAuthenticationConfig.Resource)}. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.ClientId), "client-id");
        element.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.OutputTokenVariableName), "output-token-variable-name");
        element.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.IgnoreError), "ignore-error");

        context.AddPolicy(element);
    }
}