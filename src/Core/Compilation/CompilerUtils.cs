using Microsoft.CodeAnalysis.CSharp;
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
            default:
                return "";
        }
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

        if (expressionMethod.ExpressionBody != null)
        {
            return new LambdaExpression<bool>($"context => {expressionMethod.ExpressionBody.Expression}").Source;
        }

        throw new InvalidOperationException("Invalid expression");
    }

    public static InitializerValue Process(
        this ObjectCreationExpressionSyntax creationSyntax,
        ICompilationContext context,
        IReadOnlyDictionary<string, string>? nameReplace = null)
    {
        var result = new Dictionary<string, InitializerValue>();
        foreach (var expression in creationSyntax.Initializer?.Expressions ?? [])
        {
            if (expression is not AssignmentExpressionSyntax assignment)
            {
                context.ReportError($"TODO. {expression.GetLocation()}");
                continue;
            }

            var name = assignment.Left.ToString();
            name = nameReplace?.GetValueOrDefault(name, name) ?? name;

            result[name] = assignment.Right.ProcessExpression(context, nameReplace);
        }

        return new InitializerValue
        {
            Type = (creationSyntax.Type as IdentifierNameSyntax)?.Identifier.ValueText, NamedValues = result,
        };
    }

    public static InitializerValue Process(
        this ArrayCreationExpressionSyntax creationSyntax,
        ICompilationContext context,
        IReadOnlyDictionary<string, string>? nameReplace = null)
    {
        var result = new List<InitializerValue>();
        foreach (var expression in creationSyntax.Initializer?.Expressions ?? [])
        {
            result.Add(expression.ProcessExpression(context, nameReplace));
        }

        return new InitializerValue
        {
            Type = (creationSyntax.Type.ElementType as IdentifierNameSyntax)?.Identifier.ValueText,
            UnnamedValues = result,
        };
    }

    public static InitializerValue Process(
        this CollectionExpressionSyntax collectionSyntax,
        ICompilationContext context,
        IReadOnlyDictionary<string, string>? nameReplace = null)
    {
        var result = collectionSyntax.Elements
            .OfType<ExpressionElementSyntax>()
            .Select(e => e.Expression)
            .Select(expression => expression.ProcessExpression(context, nameReplace)).ToList();

        return new InitializerValue { UnnamedValues = result, };
    }

    public static InitializerValue Process(
        this ImplicitArrayCreationExpressionSyntax creationSyntax,
        ICompilationContext context,
        IReadOnlyDictionary<string, string>? nameReplace = null)
    {
        var result = creationSyntax.Initializer.Expressions
            .Select(expression => expression.ProcessExpression(context, nameReplace))
            .ToList();

        return new InitializerValue { UnnamedValues = result, };
    }

    public static InitializerValue ProcessExpression(
        this ExpressionSyntax expression,
        ICompilationContext context,
        IReadOnlyDictionary<string, string>? nameReplace = null)
    {
        return expression switch
        {
            ObjectCreationExpressionSyntax config => config.Process(context, nameReplace),
            ArrayCreationExpressionSyntax array => array.Process(context, nameReplace),
            ImplicitArrayCreationExpressionSyntax array => array.Process(context, nameReplace),
            CollectionExpressionSyntax collection => collection.Process(context, nameReplace),
            _ => new InitializerValue { Value = expression.ProcessParameter(context) }
        };
    }
}

public class InitializerValue
{
    public string? Name { get; init; }
    public string? Value { get; init; }
    public string? Type { get; init; }
    public IReadOnlyCollection<InitializerValue>? UnnamedValues { get; init; }
    public IReadOnlyDictionary<string, InitializerValue>? NamedValues { get; init; }
}