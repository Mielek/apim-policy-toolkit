using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Analyzers;
public class ExpressionTypeAnalyzer
{
    private readonly SyntaxNodeAnalysisContext _context;
    private readonly SyntaxNode _node;
    private readonly INamedTypeSymbol _symbol;

    public ExpressionTypeAnalyzer(SyntaxNodeAnalysisContext context, SyntaxNode node, INamedTypeSymbol symbol)
    {
        _context = context;
        _node = node;
        _symbol = symbol;
    }

    public void Analyze()
    {
        var typeName = (_symbol.IsGenericType ? _symbol.OriginalDefinition : _symbol)?.ToFullyQualifiedString() ?? "";
        if (Constants.AllowedTypes.Contains(typeName))
        {
            if (Constants.AllowedInTypes.TryGetValue(typeName, out var allowed) && !allowed.Contains(_symbol.Name))
            {
                _context.ReportDiagnostic(Diagnostic.Create(Rules.DisallowedMember, _node.GetLocation(), _symbol.Name));
            }
            else if (Constants.DisallowedInTypes.TryGetValue(typeName, out var disallowed) && disallowed.Contains(_symbol.Name))
            {
                _context.ReportDiagnostic(Diagnostic.Create(Rules.DisallowedMember, _node.GetLocation(), _symbol.Name));
            }
        }
        else
        {
            _context.ReportDiagnostic(Diagnostic.Create(Rules.DisallowedType, _node.GetLocation(), typeName));
        }
    }
}