using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Model.Expressions;
using Microsoft.CodeAnalysis;

namespace Mielek.Marshalling.Expressions;

public class MethodExpressionHandler<T> : MarshallerHandler<MethodExpression<T>>
{
    public override void Marshal(Marshaller marshaller, MethodExpression<T> element)
    {
        var syntax = new TriviaRemover().Visit(CSharpSyntaxTree.ParseText(File.ReadAllText(element.FilePath)).GetRoot());

        var method = syntax.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().SingleOrDefault(s => s.Identifier.ValueText == element.MethodInfo.Name);

        if(method == null)
        {
            throw new Exception();
        }

        method = marshaller.Options.FormatCSharp
            ? method.NormalizeWhitespace()
            : method.NormalizeWhitespace("", "");

        if (method.Body != null)
        {
            marshaller.Writer.WriteRawString($"@{method.Body.ToFullString()}");
        }
        else if (method.ExpressionBody != null)
        {
            marshaller.Writer.WriteRawString($"@({method.ExpressionBody.Expression.ToFullString().TrimEnd()})");
        }
        else
        {
            throw new Exception();
        }
    }
}


public class TriviaRemover : CSharpSyntaxRewriter
{
    public override SyntaxTriviaList VisitList(SyntaxTriviaList list)
    {
        return SyntaxFactory.TriviaList();
    }
}
