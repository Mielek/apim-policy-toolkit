using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers;

public static class SyntaxExtensions
{
    public static bool ContainsAttributeOfType(this SyntaxList<AttributeListSyntax> syntax, SemanticModel model, string type)
    {
        return syntax
            .SelectMany(a => a.Attributes)
            .Any(attribute =>
            {
                var attributeType = model.GetSymbolInfo(attribute).Symbol?.ContainingType;
                return attributeType?.ToFullyQualifiedString() == type;
            });
    }

    private const string ExpressionAttribute =
        "Mielek.Azure.ApiManagement.PolicyToolkit.Attributes.ExpressionAttribute";

    public static bool ContainsExpressionAttribute(this SyntaxList<AttributeListSyntax> syntax, SemanticModel model)
    {
        return syntax.ContainsAttributeOfType(model, ExpressionAttribute);
    }

    public static bool IsPartOfPolicyExpressionMethod(this SyntaxNode syntax, SemanticModel model)
    {
        return syntax.Ancestors()
            .OfType<MethodDeclarationSyntax>()
            .Any(c => c.AttributeLists.ContainsExpressionAttribute(model));
    }

    public static bool IsPartOfPolicyExpressionDelegate(this SyntaxNode syntaxNode, SemanticModel model)
    {
        return syntaxNode.Ancestors()
            .OfType<ParenthesizedLambdaExpressionSyntax>()
            .Any(c => c.AttributeLists.ContainsExpressionAttribute(model));
    }
}