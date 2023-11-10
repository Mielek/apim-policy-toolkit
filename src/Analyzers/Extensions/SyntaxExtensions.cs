
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Analyzers;

public static class SyntaxExtensions
{
    public static bool ContainsAttributeOfType(this MemberDeclarationSyntax syntax, SemanticModel model, string type)
    {
        return syntax.AttributeLists
            .SelectMany(a => a.Attributes)
            .Any(attribute =>
            {
                var attributeType = model.GetSymbolInfo(attribute).Symbol?.ContainingType;
                return attributeType?.ToFullyQualifiedString() == type;
            });
    }
}