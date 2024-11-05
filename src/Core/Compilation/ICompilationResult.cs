using System.Xml.Linq;

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

public interface ICompilationResult
{
    XElement Document { get; }
    IReadOnlyList<string> Errors { get; }
}