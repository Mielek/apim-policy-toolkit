using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

public class SubCompilationContext : ICompilationContext
{
    private readonly ICompilationContext _parent;
    private readonly XElement _element;

    public SubCompilationContext(ICompilationContext parent, XElement element)
    {
        _parent = parent;
        _element = element;
    }

    public void AddPolicy(XNode element) => _element.Add(element);

    public void ReportError(string message) => _parent.ReportError(message);

    public SyntaxNode SyntaxRoot => _parent.SyntaxRoot;
}