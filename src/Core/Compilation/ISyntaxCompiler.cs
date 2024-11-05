using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

public interface ISyntaxCompiler
{
    SyntaxKind Syntax { get; }

    void Compile(ICompilationContext context, SyntaxNode node);
}