using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Tests.Extensions;

public static class StringExtensions
{
    public static ICompilationResult CompileDocument(this string document)
    {
        var code = CSharpSyntaxTree.ParseText(document);
        var policy = code
            .GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .First(c => c.AttributeLists.ContainsAttributeOfType("Document"));

        return new CSharpPolicyCompiler(policy).Compile();
    }
}