using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

public class ExpressionStatementCompiler : ISyntaxCompiler
{
    private IReadOnlyDictionary<string, IMethodPolicyHandler> _handlers;

    public ExpressionStatementCompiler(IEnumerable<IMethodPolicyHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.MethodName);
    }

    public SyntaxKind Syntax => SyntaxKind.ExpressionStatement;

    public void Compile(ICompilationContext context, SyntaxNode node)
    {
        var statement = node as ExpressionStatementSyntax ?? throw new NullReferenceException(nameof(node));
        var invocation = statement.Expression as InvocationExpressionSyntax;
        if (invocation == null)
        {
            context.ReportError($"{statement.Expression.GetType().Name} is not supported. {statement.Expression.GetLocation()}");
            return;
        }

        var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
        if (memberAccess == null)
        {
            context.ReportError($"{invocation.Expression.GetType().Name} is not supported. {invocation.Expression.GetLocation()}");
            return;
        }

        var name = memberAccess.Name.ToString();
        if (_handlers.TryGetValue(name, out var handler))
        {
            handler.Handle(context, invocation);
        }
        else
        {
            context.ReportError($"{name}");
        }
    }
}