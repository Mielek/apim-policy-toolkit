using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class LambdaExpressionHandler<T> : MarshallerHandler<LambdaExpression<T>>
{
    public override void Marshal(Marshaller marshaller, LambdaExpression<T> element)
    {
        var syntax = new TriviaRemover().Visit(CSharpSyntaxTree.ParseText(element.Code).GetRoot());

        var lambda = syntax.DescendantNodesAndSelf().OfType<LambdaExpressionSyntax>().FirstOrDefault();
        if (lambda == null)
        {
            throw new Exception();
        }

        lambda = marshaller.Options.FormatCSharp
            ? lambda.NormalizeWhitespace()
            : lambda.NormalizeWhitespace("", "");

        if (lambda.Block != null)
        {
            MarshallBlock(marshaller, lambda.Block);
        }
        else if (lambda.ExpressionBody != null)
        {
            MarshallExpressionBody(marshaller, lambda.ExpressionBody);
        }
        else
        {
            throw new Exception();
        }
    }
    private void MarshallBlock(Marshaller marshaller, BlockSyntax block)
    {
        marshaller.Writer.WriteRawString($"@{block.ToFullString()}");
    }

    private void MarshallExpressionBody(Marshaller marshaller, ExpressionSyntax expression)
    {
        marshaller.Writer.WriteRawString($"@({expression})");
    }

}
