using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

public class LocalDeclarationStatementCompiler : ISyntaxCompiler
{
    private IReadOnlyDictionary<string, IReturnValueMethodPolicyHandler> _handlers;

    public LocalDeclarationStatementCompiler(IEnumerable<IReturnValueMethodPolicyHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.MethodName);
    }

    public SyntaxKind Syntax => SyntaxKind.LocalDeclarationStatement;

    public void Compile(ICompilationContext context, SyntaxNode node)
    {
        var syntax = node as LocalDeclarationStatementSyntax ?? throw new Exception();
        var variables = syntax.Declaration.Variables;
        if (variables.Count > 1)
        {
            context.ReportError("");
            return;
        }

        var variable = variables[0];
        var invocation = variable.Initializer?.Value as InvocationExpressionSyntax;
        var memberAccess = invocation?.Expression as MemberAccessExpressionSyntax;
        var methodName = memberAccess?.Name.ToString();
        if (_handlers.TryGetValue(methodName, out var handler))
        {
            handler.Handle(context, invocation, variable.Identifier.ValueText);
        }
        else
        {
            context.ReportError("");
        }
    }
}