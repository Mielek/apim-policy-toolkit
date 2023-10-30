using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Model.Expressions;
using Microsoft.CodeAnalysis;
using Mielek.Model.Attributes;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Mielek.Marshalling.Expressions;

public class MethodExpressionHandler<T> : MarshallerHandler<MethodExpression<T>>
{
    public override void Marshal(Marshaller marshaller, MethodExpression<T> element)
    {
        if (!TryFindMethod(element, out var method))
        {
            throw new Exception();
        }

        method = Format(method, marshaller.Options);

        if (method.Body != null)
        {
            marshaller.Writer.WriteRawString($"@{method.Body.ToFullString()}");
        }
        else if (method.ExpressionBody != null)
        {
            marshaller.Writer.WriteRawString($"@({method.ExpressionBody.Expression.ToFullString().TrimEnd()})");
        }
        else
        {
            throw new Exception();
        }
    }

    private bool TryFindMethod(MethodExpression<T> element, [NotNullWhen(true)] out MethodDeclarationSyntax? method)
    {
        var expression = element.MethodInfo.GetCustomAttribute<ExpressionAttribute>();
        if (!string.IsNullOrEmpty(expression?.SourceFilePath))
        {
            method = FindMethod(expression.SourceFilePath, element.MethodInfo);
        }
        else if (!string.IsNullOrEmpty(element.FilePath))
        {
            method = FindMethod(element.FilePath, element.MethodInfo);
        }
        else
        {
            method = null;
        }
        
        return method != null;
    }

    private MethodDeclarationSyntax? FindMethod(string filePath, MethodInfo methodInfo)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
        return syntaxTree.GetRoot()
            .DescendantNodesAndSelf()
            .OfType<TypeDeclarationSyntax>()
            .Where(type => type.Identifier.ValueText == methodInfo.DeclaringType?.Name)
            .SelectMany(t => t.DescendantNodesAndSelf())
            .OfType<MethodDeclarationSyntax>()
            .SingleOrDefault(m => m.Identifier.ValueText == methodInfo.Name);
    }

    private MethodDeclarationSyntax Format(MethodDeclarationSyntax method, MarshallerOptions options)
    {
        var unformatted = (MethodDeclarationSyntax)new TriviaRemoverRewriter().Visit(method);

        return options.FormatCSharp
            ? unformatted.NormalizeWhitespace()
            : unformatted.NormalizeWhitespace("", "");
    }
}

