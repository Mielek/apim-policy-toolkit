using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Testing.Expressions;

public static class ExpressionProvider
{
    readonly static Regex DirectivesRegex = new Regex("^#.* .*$", RegexOptions.Multiline);

    public static Expression LoadFromFile(string path)
    {
        var code = File.ReadAllText(path);
        code = DirectivesRegex.Replace(code, "").Trim();
        return new Expression(code);
    }

    public static Dictionary<string, Expression> LoadFromFunctionFile(string path)
    {
        var code = File.ReadAllText(path);
        var scriptSyntaxTree = CSharpSyntaxTree.ParseText(code);

        return scriptSyntaxTree.GetRoot()
            .DescendantNodes()
            .OfType<LocalFunctionStatementSyntax>()
            .ToDictionary(
                function => function.Identifier.ToString(),
                function =>
                {
                    if (function.Body != null) return new Expression(function.Body.Statements.ToString());
                    if (function.ExpressionBody != null) return new Expression($"return {function.ExpressionBody.Expression};");

                    throw new Exception();
                });
    }
}