using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class FunctionFileScriptExpressionHandler<T> : MarshallerHandler<FunctionFileScriptExpression<T>>
{
    readonly Dictionary<string, string> _cache = new();

    public override void Marshal(Marshaller marshaller, FunctionFileScriptExpression<T> element)
    {
        var cacheKey = element.Path + element.Name;
        if (!_cache.ContainsKey(cacheKey))
        {
            AddFileFunctionsToCache(marshaller.Options.ScriptBaseDirectory, element.Path);
        }

        marshaller.Writer.WriteString(_cache[cacheKey]);
    }

    private void AddFileFunctionsToCache(string baseDirectory, string selectedFile)
    {
        var scriptsContent = File.ReadAllText(Path.Combine(baseDirectory, selectedFile));
        var scriptSyntaxTree = CSharpSyntaxTree.ParseText(scriptsContent);
        var functions = scriptSyntaxTree.GetRoot().DescendantNodes().OfType<LocalFunctionStatementSyntax>();

        foreach (var function in functions)
        {
            var cacheId = selectedFile + function.Identifier.ToString();

            if (function.ExpressionBody != null)
            {
                _cache[cacheId] = $"@({function.ExpressionBody.Expression})";
            }
            else if (function.Body != null)
            {
                var functionStatements = function.Body.Statements;
                if (functionStatements.Count > 1)
                {
                    _cache[cacheId] = $"@{{{Environment.NewLine}{functionStatements}{Environment.NewLine}}}";
                }
                else
                {
                    var returnStatement = (ReturnStatementSyntax) functionStatements[0];
                    _cache[cacheId] = $"@({returnStatement.Expression})";
                }
            }
        }
    }
}