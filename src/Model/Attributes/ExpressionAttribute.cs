using System.Runtime.CompilerServices;

namespace Mielek.Model.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ExpressionAttribute : Attribute
{

    public string SourceFilePath { get; }

    public ExpressionAttribute([CallerFilePath] string sourceFilePath = "")
    {
        SourceFilePath = sourceFilePath;
    }
}