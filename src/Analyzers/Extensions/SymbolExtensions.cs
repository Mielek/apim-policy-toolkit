
using Microsoft.CodeAnalysis;

namespace Mielek.Analyzers;

public static class SymbolExtensions
{
    private readonly static SymbolDisplayFormat Format = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
    public static string ToFullyQualifiedString(this ISymbol symbol) => symbol.ToDisplayString(Format);
}