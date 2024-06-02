using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

public class InvocationExpressionCompiler : ISyntaxCompiler
{
    private IReadOnlyDictionary<string, IMethodPolicyHandler> _handlers;

    public InvocationExpressionCompiler(IEnumerable<IMethodPolicyHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.MethodName);
    }

    public SyntaxKind Syntax => SyntaxKind.InvocationExpression;
    public void Compile(ICompilationContext context, SyntaxNode node)
    {
        var invocation = node as InvocationExpressionSyntax ?? throw new NullReferenceException();
        var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
        var name = memberAccess.Name.ToString();
        if (_handlers.TryGetValue(name, out var handler))
        {
            handler.Handle(context, invocation);
        }
        else
        {
            context.ReportError("");
        }
    }
}