using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public static class SyntaxExtensions
{

    public static bool ContainsAttributeOfType(this SyntaxList<AttributeListSyntax> syntax, string type)
    {
        return syntax.GetFirstAttributeOfType(type) != null;
    }

    public static AttributeSyntax? GetFirstAttributeOfType(this SyntaxList<AttributeListSyntax> syntax, string type)
    {
        return syntax
            .SelectMany(a => a.Attributes)
            .FirstOrDefault(attribute => string.Equals(attribute.Name.ToString(), type, StringComparison.Ordinal));
    }
}