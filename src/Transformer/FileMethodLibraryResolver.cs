using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Model.Expressions;

public class FileMethodLibraryResolver
{

    SyntaxTree tree;
    CompilationUnitSyntax root;

    public FileMethodLibraryResolver(string code)
    {
        tree = CSharpSyntaxTree.ParseText(code);
        root = tree.GetCompilationUnitRoot();
    }

    public IDictionary<string, string> ResolveMethodLibrary()
    {
        var lib = new Dictionary<string, string>();
        foreach (var item in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            if (item.AttributeLists.SelectMany(l => l.Attributes).Any(a => a.Name.ToString() == nameof(MethodExpressionAttribute) || a.Name.ToString() == "MethodExpression"))
            {
                var body = item.Body?.Statements.ToString();
                var expression = item.ExpressionBody?.Expression.ToString();
                lib.Add(item.Identifier.ToString(), body ?? expression ?? "");
            }
        }

        foreach (var item in root.DescendantNodes().OfType<LambdaExpressionSyntax>())
        {
            var att = item.AttributeLists.SelectMany(l => l.Attributes).Where(a => a.Name.ToString() == nameof(LambdaExpressionAttribute) || a.Name.ToString() == "LambdaExpressionAttribute").SingleOrDefault();
            if (att != null)
            {
                var key = att.ArgumentList?.Arguments.ToString() ?? "";
                var body = item.Block?.Statements.ToString();
                var expression = item.ExpressionBody?.ToString();
                lib.Add(key, body ?? expression ?? "");
            }
        }

        return lib;
    }
}