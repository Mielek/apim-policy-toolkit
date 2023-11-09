using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class TypeUsedAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Rules.TypeUsed.All;

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression,
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxKind.ElementAccessExpression,
                SyntaxKind.ObjectCreationExpression,
                SyntaxKind.ObjectInitializerExpression,
                SyntaxKind.AnonymousObjectCreationExpression);
        context.EnableConcurrentExecution();
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var node = context.Node;
        var nodeSymbol = context.SemanticModel.GetSymbolInfo(node).Symbol;
        if (nodeSymbol == null)
        {
            return;
        }

        var symbol = nodeSymbol.ContainingType;
        var typeName = (symbol.IsGenericType ? symbol.OriginalDefinition : symbol)?.ToFullyQualifiedString() ?? "";
        if (Constants.AllowedTypes.Contains(typeName))
        {
            if (Constants.AllowedInTypes.TryGetValue(typeName, out var allowed) && !allowed.Contains(symbol.Name))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.TypeUsed.DisallowedMember, node.GetLocation(), symbol.Name));
            }
            else if (Constants.DisallowedInTypes.TryGetValue(typeName, out var disallowed) && disallowed.Contains(symbol.Name))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.TypeUsed.DisallowedMember, node.GetLocation(), symbol.Name));
            }
        }
        else
        {
            context.ReportDiagnostic(Diagnostic.Create(Rules.TypeUsed.DisallowedType, node.GetLocation(), typeName));
        }
    }


}
