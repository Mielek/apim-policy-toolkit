using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public static class CompilerUtils
{
    public static string ProcessParameter(ICompilationContext context, ExpressionSyntax expression)
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
                return FindCode(context, syntax);
        }

        return "";
    }
    
    public static string FindCode(ICompilationContext context, InvocationExpressionSyntax syntax)
    {
        var methodIdentifier = (syntax.Expression as IdentifierNameSyntax).Identifier.ValueText;
        var expressionMethod = context.Root.DescendantNodes()
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
}