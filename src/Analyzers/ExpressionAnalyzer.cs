using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExpressionAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Rules.All;

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(context => new ExpressionMethodAnalyzer(context).Analyze(), SyntaxKind.MethodDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.ObjectCreationExpression);
        context.EnableConcurrentExecution();
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var method = context.ContainingSymbol as IMethodSymbol;
        if (method == null) return;
        if (!method.GetAttributes().Any(a => a.AttributeClass?.ToFullyQualifiedString() == Constants.Method.ExpressionAttribute)) return;

        var node = context.Node;
        var symbol = context.SemanticModel.GetSymbolInfo(node).Symbol;
        if (symbol != null)
        {
            new ExpressionTypeAnalyzer(context, node, symbol.ContainingType).Analyze();
        }
    }


}
