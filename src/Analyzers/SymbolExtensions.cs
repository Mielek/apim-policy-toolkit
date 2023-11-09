
using Microsoft.CodeAnalysis;

namespace Mielek.Analyzers;

public static class SymbolExtensions
{
    public static string ToFullyQualifiedString(this ISymbol symbol)
        => symbol.ToDisplayString(Constants.format);
}