using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

public interface ICompilationContext
{
    void AddPolicy(XNode element);
    void ReportError(string message);

    SyntaxNode SyntaxRoot { get; }
}