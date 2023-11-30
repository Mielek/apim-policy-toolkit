using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Model.Attributes;

namespace Mielek.Builders.Expressions;

public sealed record ConstantExpression<T>(T Value) : IExpression<T>
{
    public XText GetXText() => new XText($"{Value}");
}
public sealed record InlineExpression<T>(string Expression) : IExpression<T>
{
    public XText GetXText() => new XText(Expression);
}
public sealed record LambdaExpression<T>(MethodInfo LambdaInfo, string Code) : IExpression<T>
{
    public XText GetXText() => new XText(Serialize());

    private string Serialize()
    {
        if (!TryGetLambda(out var lambda))
        {
            throw new Exception();
        }

        lambda = Format(lambda);

        if (lambda.Block != null)
        {
            return $"@{lambda.Block.ToFullString().TrimEnd()}";
        }
        else if (lambda.ExpressionBody != null)
        {
            return $"@({lambda.ExpressionBody})";
        }
        else
        {
            throw new Exception();
        }
    }

    private bool TryGetLambda([NotNullWhen(true)] out LambdaExpressionSyntax? lambda)
    {
        var syntax = CSharpSyntaxTree.ParseText(Code).GetRoot();
        lambda = syntax.DescendantNodesAndSelf().OfType<LambdaExpressionSyntax>().FirstOrDefault();
        return lambda != null;
    }

    private LambdaExpressionSyntax Format(LambdaExpressionSyntax lambda)
    {
        var unformatted = (LambdaExpressionSyntax)new TriviaRemoverRewriter().Visit(lambda);
        return unformatted.NormalizeWhitespace("", "");
        // return options.FormatCSharp
        //     ? unformatted.NormalizeWhitespace()
        //     : unformatted.NormalizeWhitespace("", "");
    }
}
public sealed record MethodExpression<T>(MethodInfo MethodInfo, string FilePath) : IExpression<T>
{
    public XText GetXText() => new XText(Serialize());

    private string Serialize()
    {
        if (!TryFindMethod(out var method))
        {
            throw new Exception();
        }

        method = Format(method);

        if (method.Body != null)
        {
            return $"@{method.Body.ToFullString()}";
        }
        else if (method.ExpressionBody != null)
        {
            return $"@({method.ExpressionBody.Expression.ToFullString().TrimEnd()})";
        }
        else
        {
            throw new Exception();
        }
    }


    private bool TryFindMethod([NotNullWhen(true)] out MethodDeclarationSyntax? method)
    {
        var expression = MethodInfo.GetCustomAttribute<ExpressionAttribute>();
        if (!string.IsNullOrEmpty(expression?.SourceFilePath))
        {
            method = FindMethod(expression.SourceFilePath, MethodInfo);
        }
        else if (!string.IsNullOrEmpty(FilePath))
        {
            method = FindMethod(FilePath, MethodInfo);
        }
        else
        {
            method = null;
        }

        return method != null;
    }

    private MethodDeclarationSyntax? FindMethod(string filePath, MethodInfo methodInfo)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
        return syntaxTree.GetRoot()
            .DescendantNodesAndSelf()
            .OfType<TypeDeclarationSyntax>()
            .Where(type => type.Identifier.ValueText == methodInfo.DeclaringType?.Name)
            .SelectMany(t => t.DescendantNodesAndSelf())
            .OfType<MethodDeclarationSyntax>()
            .SingleOrDefault(m => m.Identifier.ValueText == methodInfo.Name);
    }

    private MethodDeclarationSyntax Format(MethodDeclarationSyntax method)
    {
        var unformatted = (MethodDeclarationSyntax)new TriviaRemoverRewriter().Visit(method);

        return unformatted.NormalizeWhitespace("", "");
        // return options.FormatCSharp
        //     ? unformatted.NormalizeWhitespace()
        //     : unformatted.NormalizeWhitespace("", "");
    }
}
