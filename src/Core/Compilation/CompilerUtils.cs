using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
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
        ICompilationContext context)
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
            result[name] = assignment.Right.ProcessExpression(context);
        }

        return new InitializerValue
        {
            Type = (creationSyntax.Type as IdentifierNameSyntax)?.Identifier.ValueText,
            NamedValues = result,
            Node = creationSyntax,
        };
    }

    public static InitializerValue Process(
        this ArrayCreationExpressionSyntax creationSyntax,
        ICompilationContext context)
    {
        var expressions = creationSyntax.Initializer?.Expressions ?? [];
        var result = expressions
        .Select(expression => expression.ProcessExpression(context))
        .ToList();
        
        return new InitializerValue
        {
            Type = (creationSyntax.Type.ElementType as IdentifierNameSyntax)?.Identifier.ValueText,
            UnnamedValues = result,
            Node = creationSyntax,
        };
    }

    public static InitializerValue Process(
        this CollectionExpressionSyntax collectionSyntax,
        ICompilationContext context)
    {
        var result = collectionSyntax.Elements
            .OfType<ExpressionElementSyntax>()
            .Select(e => e.Expression)
            .Select(expression => expression.ProcessExpression(context)).ToList();

        return new InitializerValue { UnnamedValues = result, Node = collectionSyntax };
    }

    public static InitializerValue Process(
        this ImplicitArrayCreationExpressionSyntax creationSyntax,
        ICompilationContext context)
    {
        var result = creationSyntax.Initializer.Expressions
            .Select(expression => expression.ProcessExpression(context))
            .ToList();

        return new InitializerValue { UnnamedValues = result, Node = creationSyntax };
    }

    public static InitializerValue ProcessExpression(
        this ExpressionSyntax expression,
        ICompilationContext context)
    {
        return expression switch
        {
            ObjectCreationExpressionSyntax config => config.Process(context),
            ArrayCreationExpressionSyntax array => array.Process(context),
            ImplicitArrayCreationExpressionSyntax array => array.Process(context),
            CollectionExpressionSyntax collection => collection.Process(context),
            _ => new InitializerValue { Value = expression.ProcessParameter(context), Node = expression }
        };
    }

    public static bool AddAttribute(this XElement element, IReadOnlyDictionary<string, InitializerValue> parameters, string key, string attName)
    {
        if(parameters.TryGetValue(key, out var value))
        {
            element.Add(new XAttribute(attName, value.Value!));
            return true;
        }
        return false;
    }

    public static bool TryExtractingConfigParameter<T>(
        this InvocationExpressionSyntax node,
        ICompilationContext context,
        string policy,
        [NotNullWhen(true)] out IReadOnlyDictionary<string, InitializerValue>? values)
    {
        values = null;

        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for {policy} policy. {node.GetLocation()}");
            return false;
        }

        return node.ArgumentList.Arguments[0].Expression.TryExtractingConfig<T>(context, policy, out values);
    }

    public static bool TryExtractingConfig<T>(this ExpressionSyntax syntax,
        ICompilationContext context,
        string policy,
        [NotNullWhen(true)] out IReadOnlyDictionary<string, InitializerValue>? values)
    {
        values = null;
        if (syntax is not ObjectCreationExpressionSyntax config)
        {
            context.ReportError($"{policy} policy argument must be an object creation expression. {syntax.GetLocation()}");
            return false;
        }
        var initializer = config.Process(context);
        if (!initializer.TryGetValues<T>(out var result))
        {
            context.ReportError($"{policy} policy argument must be of type {typeof(T).Name}. {syntax.GetLocation()}");
            return false;
        }

        values = result;
        return true;
    }

    public static string ExtractDocumentFileName(this ClassDeclarationSyntax document)
    {
        var attributeSyntax = document.AttributeLists.GetFirstAttributeOfType("Document");
        var attributeArgumentExpression =
            attributeSyntax?.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax;
        return attributeArgumentExpression?.Token.ValueText ?? document.Identifier.ValueText;
    }
    
}

public class InitializerValue
{
    public string? Name { get; init; }
    public string? Value { get; init; }
    public string? Type { get; init; }
    public IReadOnlyCollection<InitializerValue>? UnnamedValues { get; init; }
    public IReadOnlyDictionary<string, InitializerValue>? NamedValues { get; init; }

    public required SyntaxNode Node { get; init; }

    public bool TryGetValues<T>([NotNullWhen(true)] out IReadOnlyDictionary<string, InitializerValue>? namedValues)
    {
        if (Type == typeof(T).Name && NamedValues is not null)
        {
            namedValues = NamedValues;
            return true;
        }
        namedValues = null;
        return false;
    }
}