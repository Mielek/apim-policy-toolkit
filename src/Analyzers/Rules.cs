using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace Mielek.Analyzers;

public static class Rules
{
    public readonly static DiagnosticDescriptor DEBUG = new DiagnosticDescriptor(
        "DEBUG",
        "DEBUG",
        "{0}",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new[] { "APIM", "ApiManagement" });

    public readonly static DiagnosticDescriptor ReturnValue = new DiagnosticDescriptor(
        "APIM001",
        "Disallowed expression method return type",
        "Method returns '{0}' which is not an allowed expression return type",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new[] { "APIM", "ApiManagement" });
    public readonly static DiagnosticDescriptor ParameterLength = new DiagnosticDescriptor(
        "APIM002",
        "Too many parameters in expression method",
        "Method declares '{0}' parameters which is not allowed for an expression",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new[] { "APIM", "ApiManagement" });
    public readonly static DiagnosticDescriptor ParameterType = new DiagnosticDescriptor(
        "APIM003",
        "Wrong expression method parameter type",
        "Method declares '{0}' parameter type which is not allowed for an expression. Parameter should be of '{1}' type.",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new[] { "APIM", "ApiManagement" });
    public readonly static DiagnosticDescriptor ParameterName = new DiagnosticDescriptor(
        "APIM004",
        "Wrong expression method parameter name",
        "Method declares parameter with '{0}' name which is not allowed for an expression. Parameter should have '{1}' name.",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new[] { "APIM", "ApiManagement" });

    public readonly static DiagnosticDescriptor DisallowedType = new DiagnosticDescriptor(
        "APIM005",
        "Disallowed type used in expression method",
        "Type '{0}' is not allowed in expression method",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new[] { "APIM", "ApiManagement" });

    public readonly static DiagnosticDescriptor DisallowedMember = new DiagnosticDescriptor(
        "APIM005",
        "Disallowed member used in expression method",
        "Type '{0}' is not allowed in expression method",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new[] { "APIM", "ApiManagement" });

    public readonly static ImmutableArray<DiagnosticDescriptor> All = ImmutableArray.Create(
        DEBUG, ReturnValue, ParameterLength, ParameterName, ParameterType, DisallowedType, DisallowedMember
    );
}