

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using Mielek.Expressions.Context;
using Mielek.Model.Attributes;

namespace Mielek.Expressions.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExpressionAnalyzer : DiagnosticAnalyzer
{
    private readonly static DiagnosticDescriptor s_returnValueRule = new DiagnosticDescriptor(
        "APIM001",
        "Expression method should return a value",
        "Expression method should return a value",
        "Expression",
         DiagnosticSeverity.Error,
         isEnabledByDefault: true,
         description: "Description.",
         helpLinkUri: "TODO",
         customTags: new [] { "APIM", "ApiManagement" });
    private readonly static DiagnosticDescriptor s_parameterLength = new DiagnosticDescriptor(
        "APIM002",
        "Expression method should have only one parameter",
        "Expression method should have only one parameter",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new [] { "APIM", "ApiManagement" });
    private readonly static DiagnosticDescriptor s_parameterType = new DiagnosticDescriptor(
        "APIM003",
        "Expression method parameter should be IContext type",
        "Expression method parameter should be IContext type",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new [] { "APIM", "ApiManagement" });
    private readonly static DiagnosticDescriptor s_parameterName = new DiagnosticDescriptor(
        "APIM004",
        "Expression method parameter name should be 'context'",
        "Expression method parameter name should be 'context'",
        "Expression",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Description.",
        helpLinkUri: "TODO",
        customTags: new [] { "APIM", "ApiManagement" });

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_returnValueRule, s_parameterLength, s_parameterType, s_parameterName);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSymbolAction(OnMethod, SymbolKind.Method);
        context.EnableConcurrentExecution();
    }

    private void OnMethod(SymbolAnalysisContext context)
    {
        var symbol = context.Symbol;
        if (symbol.Kind != SymbolKind.Method || symbol is not IMethodSymbol methodSymbol) return;

        var attributes = methodSymbol.GetAttributes();
        var attribute = attributes.FirstOrDefault(attribute => attribute.AttributeClass?.Name == nameof(ExpressionAttribute));
        if (attribute == null) return;

        if (methodSymbol.ReturnsVoid)
        {
            context.ReportDiagnostic(Diagnostic.Create(s_returnValueRule, methodSymbol.Locations.FirstOrDefault()));
        }

        if (methodSymbol.Parameters.Length != 1)
        {
            context.ReportDiagnostic(Diagnostic.Create(s_parameterLength, methodSymbol.Locations.FirstOrDefault()));
            return;
        }

        var parameter = methodSymbol.Parameters[0];
        if (parameter.Type.Name != nameof(IContext))
        {
            context.ReportDiagnostic(Diagnostic.Create(s_parameterType, parameter.Locations.FirstOrDefault()));
        }

        if (parameter.Name != "context")
        {
            context.ReportDiagnostic(Diagnostic.Create(s_parameterName, parameter.Locations.FirstOrDefault()));
        }
    }
}