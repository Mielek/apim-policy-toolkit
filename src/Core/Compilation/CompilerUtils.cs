using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public static class CompilerUtils
{
    public static string ProcessParameter(this ExpressionSyntax expression, ICompilationContext context)
    {
        switch (expression)
        {
            case LiteralExpressionSyntax syntax:
                return syntax.Token.ValueText;
            case InterpolatedStringExpressionSyntax syntax:
                var interpolationParts = syntax.Contents.Select(c => c switch
                {
                    InterpolatedStringTextSyntax text => text.TextToken.ValueText,
                    InterpolationSyntax interpolation =>
                        $"{{context.Variables[\"{interpolation.Expression.ToString()}\"]}}",
                    _ => ""
                });
                return new LambdaExpression<string>($"context => $\"{string.Join("", interpolationParts)}\"").Source;
            case AnonymousFunctionExpressionSyntax syntax:
                return new LambdaExpression<string>(syntax.ToString()).Source;
            case InvocationExpressionSyntax syntax:
                return FindCode(syntax, context);
        }

        return "";
    }

    public static string FindCode(this InvocationExpressionSyntax syntax, ICompilationContext context)
    {
        var methodIdentifier = (syntax.Expression as IdentifierNameSyntax).Identifier.ValueText;
        var expressionMethod = context.SyntaxRoot.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .First(m => m.Identifier.ValueText == methodIdentifier);

        if (expressionMethod.Body != null)
        {
            return new LambdaExpression<bool>($"context => {expressionMethod.Body}").Source;
        }
        else if (expressionMethod.ExpressionBody != null)
        {
            return new LambdaExpression<bool>($"context => {expressionMethod.ExpressionBody.Expression}").Source;
        }
        else
        {
            throw new InvalidOperationException("Invalid expression");
        }
    }

    public static IReadOnlyDictionary<string, string> ProcessInitializerExpression(
        this InitializerExpressionSyntax initializeSyntax, ICompilationContext context,
        IReadOnlyDictionary<string, string>? nameReplace = null)
    {
        var result = new Dictionary<string, string>();
        foreach (var expression in initializeSyntax.Expressions)
        {
            if (expression is not AssignmentExpressionSyntax assignment)
            {
                context.ReportError(
                    $"Forward request policy argument must be an object initializer. {expression.GetLocation()}");
                continue;
            }

            var name = assignment.Left.ToString();
            name = nameReplace?.GetValueOrDefault(name, name) ?? name;

            var value = assignment.Right.ProcessParameter(context);
            result[name] = value;
        }

        return result;
    }
}