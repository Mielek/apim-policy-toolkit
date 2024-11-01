using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public class TriviaRemoverRewriter : CSharpSyntaxRewriter
{
    public override SyntaxTriviaList VisitList(SyntaxTriviaList list)
    {
        return SyntaxFactory.TriviaList();
    }
}