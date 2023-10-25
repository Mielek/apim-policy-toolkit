using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class LambdaExpressionHandler<T> : MarshallerHandler<LambdaExpression<T>>
{
    public override void Marshal(Marshaller marshaller, LambdaExpression<T> element)
    {
        if (!TryGetLambda(element, out var lambda))
        {
            throw new Exception();
        }

        lambda = Format(lambda, marshaller.Options);

        if (lambda.Block != null)
        {
            marshaller.Writer.WriteRawString($"@{lambda.Block.ToFullString().TrimEnd()}");
        }
        else if (lambda.ExpressionBody != null)
        {
            marshaller.Writer.WriteRawString($"@({lambda.ExpressionBody})");
        }
        else
        {
            throw new Exception();
        }
    }

    private bool TryGetLambda(LambdaExpression<T> element, [NotNullWhen(true)] out LambdaExpressionSyntax? lambda)
    {
        var syntax = CSharpSyntaxTree.ParseText(element.Code).GetRoot();
        lambda = syntax.DescendantNodesAndSelf().OfType<LambdaExpressionSyntax>().FirstOrDefault();
        return lambda != null;
    }

    private LambdaExpressionSyntax Format(LambdaExpressionSyntax lambda, MarshallerOptions options)
    {
        var unformatted = (LambdaExpressionSyntax)new TriviaRemoverRewriter().Visit(lambda);
        return options.FormatCSharp
            ? unformatted.NormalizeWhitespace()
            : unformatted.NormalizeWhitespace("", "");
    }
}
