// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

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
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "authentication-managed-identity",
                nameof(ManagedIdentityAuthenticationConfig.Resource)
                ));
            return;
        }

        element.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.ClientId), "client-id");
        element.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.OutputTokenVariableName), "output-token-variable-name");
        element.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.IgnoreError), "ignore-error");

        context.AddPolicy(element);
    }
}