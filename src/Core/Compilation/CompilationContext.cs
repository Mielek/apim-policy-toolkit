using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public class CompilationContext : ICompilationContext
{
    public IList<string> Errors = new List<string>();
    private XElement _section;
    
    public CompilationContext(SyntaxNode root, XElement section)
    {
        Root = root;
        this._section = section;
    }
    
    public void AddPolicy(XElement element) => _section.Add(element);

    public void ReportError(string message) => Errors.Add(message);

    public SyntaxNode Root { get; }
}