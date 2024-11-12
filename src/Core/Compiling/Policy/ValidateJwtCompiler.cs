// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class ValidateJwtCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ValidateJwt);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ValidateJwtConfig>(context, "validate-jwt", out var values))
        {
            return;
        }

        var element = new XElement("validate-jwt");

        if (new[]
            {
                element.AddAttribute(values, nameof(ValidateJwtConfig.HeaderName), "header-name"),
                element.AddAttribute(values, nameof(ValidateJwtConfig.QueryParameterName), "query-parameter-name"),
                element.AddAttribute(values, nameof(ValidateJwtConfig.TokenValue), "token-value"),
            }.Count(b => b) != 1)
        {
            context.ReportError("Only one of HeaderName, QueryParameterName and TokenValue must be set");
            return;
        }

        element.AddAttribute(values, nameof(ValidateJwtConfig.FailedValidationHttpCode), "failed-validation-httpcode");
        element.AddAttribute(values, nameof(ValidateJwtConfig.FailedValidationErrorMessage),
            "failed-validation-error-message");
        element.AddAttribute(values, nameof(ValidateJwtConfig.RequireExpirationTime), "require-expiration-time");
        element.AddAttribute(values, nameof(ValidateJwtConfig.RequireScheme), "require-scheme");
        element.AddAttribute(values, nameof(ValidateJwtConfig.RequireSignedTokens), "require-signed-tokens");
        element.AddAttribute(values, nameof(ValidateJwtConfig.ClockSkew), "clock-skew");
        element.AddAttribute(values, nameof(ValidateJwtConfig.OutputTokenVariableName), "output-token-variable-name");

        if (values.TryGetValue(nameof(ValidateJwtConfig.OpenIdConfigs), out var openIdConfigs))
        {
            var openIdElements = HandleOpenIdConfigs(context, openIdConfigs);
            element.Add(openIdElements);
        }

        HandleKeys(context, element, values, nameof(ValidateJwtConfig.IssuerSigningKeys), "issuer-signing-keys");
        HandleKeys(context, element, values, nameof(ValidateJwtConfig.DescriptionKeys), "decryption-keys");
        HandleList(element, values, nameof(ValidateJwtConfig.Audiences), "audiences", "audience");
        HandleList(element, values, nameof(ValidateJwtConfig.Issuers), "issuers", "issuer");

        if (values.TryGetValue(nameof(ValidateJwtConfig.RequiredClaims), out var requiredClaims))
        {
            XElement claimsElement = HandleRequiredClaims(context, requiredClaims);
            element.Add(claimsElement);
        }

        context.AddPolicy(element);
    }

    private static object[] HandleOpenIdConfigs(ICompilationContext context, InitializerValue openIdConfigs)
    {
        var openIdElements = new List<object>();
        foreach (var openIdConfig in openIdConfigs.UnnamedValues ?? [])
        {
            if (!openIdConfig.TryGetValues<OpenIdConfig>(out var openIdConfigValues))
            {
                continue;
            }

            var openIdElement = new XElement("openid-config");
            if (!openIdElement.AddAttribute(openIdConfigValues, nameof(OpenIdConfig.Url), "url"))
            {
                context.ReportError($"{nameof(OpenIdConfig.Url)}. {openIdConfig.Node.GetLocation()}");
                continue;
            }

            openIdElements.Add(openIdElement);
        }

        return openIdElements.ToArray();
    }

    private static XElement HandleRequiredClaims(ICompilationContext context, InitializerValue requiredClaims)
    {
        var claimsElement = new XElement("required-claims");
        foreach (var claim in requiredClaims.UnnamedValues ?? [])
        {
            if (!claim.TryGetValues<ClaimConfig>(out var claimValue))
            {
                continue;
            }

            var claimElement = new XElement("claim");
            if (!claimElement.AddAttribute(claimValue, nameof(ClaimConfig.Name), "name"))
            {
                context.ReportError($"{nameof(ClaimConfig.Name)}. {claim.Node.GetLocation()}");
                continue;
            }

            claimElement.AddAttribute(claimValue, nameof(ClaimConfig.Match), "match");
            claimElement.AddAttribute(claimValue, nameof(ClaimConfig.Separator), "separator");

            if (claimValue.TryGetValue(nameof(ClaimConfig.Values), out var valuesInitializer))
            {
                foreach (var value in valuesInitializer.UnnamedValues ?? [])
                {
                    claimElement.Add(new XElement("value", value.Value!));
                }
            }

            claimsElement.Add(claimElement);
        }

        return claimsElement;
    }

    private static void HandleKeys(
        ICompilationContext context,
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        string key,
        string listName)
    {
        if (!values.TryGetValue(key, out var listInitializer))
        {
            return;
        }

        var listElement = new XElement(listName);
        foreach (var initializer in listInitializer.UnnamedValues ?? [])
        {
            var keyValues = initializer.NamedValues;
            if (keyValues is null)
            {
                continue;
            }

            var keyElement = new XElement("key");
            keyElement.AddAttribute(keyValues, nameof(KeyConfig.Id), "id");
            switch (initializer.Type)
            {
                case nameof(Base64KeyConfig):
                    if (!keyValues.TryGetValue(nameof(Base64KeyConfig.Value), out var value))
                    {
                        context.ReportError(
                            $"{nameof(Base64KeyConfig.Value)} is required. {initializer.Node.GetLocation()}");
                        continue;
                    }
                    keyElement.Value = value.Value!;
                    break;
                case nameof(CertificateKeyConfig):
                    if (!keyElement.AddAttribute(keyValues, nameof(CertificateKeyConfig.CertificateId),
                            "certificate-id"))
                    {
                        context.ReportError(
                            $"{nameof(CertificateKeyConfig.CertificateId)} is required. {initializer.Node.GetLocation()}");
                        continue;
                    }
                    break;
                case nameof(AsymmetricKeyConfig):
                    var containsModulus =
                        keyElement.AddAttribute(keyValues, nameof(AsymmetricKeyConfig.Modulus), "n");
                    var containsExponent =
                        keyElement.AddAttribute(keyValues, nameof(AsymmetricKeyConfig.Exponent), "e");
                    if (!(containsModulus && containsExponent))
                    {
                        context.ReportError(
                            $"Modulus and Exponent are required. {initializer.Node.GetLocation()}");
                        continue;
                    }
                    break;
                default:
                    context.ReportError($"Unknown key type {initializer.Type}. {initializer.Node.GetLocation()}");
                    continue;
            }
            listElement.Add(keyElement);
        }

        element.Add(listElement);
    }

    private static void HandleList(
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        string key,
        string listName,
        string elementName)
    {
        if (!values.TryGetValue(key, out var listInitializer))
        {
            return;
        }

        var listElement = new XElement(listName);
        foreach (var initializer in listInitializer.UnnamedValues ?? [])
        {
            listElement.Add(new XElement(elementName, initializer.Value!));
        }

        element.Add(listElement);
    }
}