using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

public class TriviaRemoverRewriter : CSharpSyntaxRewriter
{
    public override SyntaxTriviaList VisitList(SyntaxTriviaList list)
    {
        return SyntaxFactory.TriviaList();
    }
}