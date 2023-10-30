

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Expressions.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExpressionAnalyzer : DiagnosticAnalyzer
{

    private static DiagnosticDescriptor Rule = new DiagnosticDescriptor("test", "Title", "MessageFormat", "Category", DiagnosticSeverity.Warning, isEnabledByDefault: true, description: "Description.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(OnMethod, MethodDeclarationSyntax);
    }

    private void OnMethod(SymbolAnalysisContext context)
    {

    }
}