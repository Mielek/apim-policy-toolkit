using System.Runtime.CompilerServices;

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

[AttributeUsage(AttributeTargets.Method)]
public class ExpressionAttribute([CallerFilePath] string sourceFilePath = "") : Attribute
{
    public string SourceFilePath { get; } = sourceFilePath;
}