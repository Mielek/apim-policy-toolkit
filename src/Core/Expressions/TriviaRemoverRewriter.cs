using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

public class TriviaRemoverRewriter : CSharpSyntaxRewriter
{
    public override SyntaxTriviaList VisitList(SyntaxTriviaList list)
    {
        return SyntaxFactory.TriviaList();
    }
}