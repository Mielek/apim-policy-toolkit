using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Testing.Expressions;

public static class ExpressionProvider
{
    readonly static Regex DirectivesRegex = new Regex("^#.* .*$", RegexOptions.Multiline);

    public static Expression<T> LoadFromFile<T>(string path)
    {
        var code = File.ReadAllText(path);
        code = DirectivesRegex.Replace(code, "").Trim();
        return new Expression<T>(code);
    }

    public static Dictionary<string, Expression<object>> LoadFromFunctionFile(string path)
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
                    if (function.Body != null) return new Expression<object>(function.Body.Statements.ToString());
                    if (function.ExpressionBody != null) return new Expression<object>($"return {function.ExpressionBody.Expression};");

                    throw new Exception();
                });
    }
}