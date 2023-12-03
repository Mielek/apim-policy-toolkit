using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

public sealed record MethodExpression<T>(MethodInfo MethodInfo, string FilePath) : IExpression<T>
{
    public string Source => Serialize();

    public XText GetXText() => new XText(Source);

    public XAttribute GetXAttribute(XName name) => new XAttribute(name, Source);

    private string Serialize()
    {
        if (!TryFindMethod(out var method))
        {
            throw new Exception();
        }

        method = Format(method);

        if (method.Body != null)
        {
            return $"@{method.Body.ToFullString()}";
        }
        else if (method.ExpressionBody != null)
        {
            return $"@({method.ExpressionBody.Expression.ToFullString().TrimEnd()})";
        }
        else
        {
            throw new Exception();
        }
    }


    private bool TryFindMethod([NotNullWhen(true)] out MethodDeclarationSyntax? method)
    {
        var expression = MethodInfo.GetCustomAttribute<ExpressionAttribute>();
        if (!string.IsNullOrEmpty(expression?.SourceFilePath))
        {
            method = FindMethod(expression.SourceFilePath, MethodInfo);
        }
        else if (!string.IsNullOrEmpty(FilePath))
        {
            method = FindMethod(FilePath, MethodInfo);
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

    private MethodDeclarationSyntax Format(MethodDeclarationSyntax method)
    {
        var unformatted = (MethodDeclarationSyntax)new TriviaRemoverRewriter().Visit(method);

        return unformatted.NormalizeWhitespace("", "");
        // return options.FormatCSharp
        //     ? unformatted.NormalizeWhitespace()
        //     : unformatted.NormalizeWhitespace("", "");
    }
}
