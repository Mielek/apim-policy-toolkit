using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace Mielek.Analyzers;

public static partial class Rules
{
    public static class PolicyDocument
    {
        public readonly static DiagnosticDescriptor ReturnValue = new DiagnosticDescriptor(
            "APIM201",
            "Disallowed document method return type",
            "Method returns '{0}' which is not an allowed document return type",
            "PolicyDocument",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Description.",
            helpLinkUri: "TODO",
            customTags: new[] { "APIM", "ApiManagement"});

        public readonly static DiagnosticDescriptor NoParametersAllowed = new DiagnosticDescriptor(
            "APIM202",
            "No parameters are allowed",
            "Document method should not accept any parameters but defines '{0}'",
            "PolicyDocument",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Description.",
            helpLinkUri: "TODO",
            customTags: new[] { "APIM", "ApiManagement" });

            
        public readonly static ImmutableArray<DiagnosticDescriptor> All = ImmutableArray.Create(
            ReturnValue, NoParametersAllowed
        );
    }
}