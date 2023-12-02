using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Builders.Expressions;

public sealed record LambdaExpression<T>(MethodInfo LambdaInfo, string Code) : IExpression<T>
{
    public string Source => Serialize();

    public XText GetXText() => new XText(Source);

    public XAttribute GetXAttribute(XName name) => new XAttribute(name, Source);

    

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