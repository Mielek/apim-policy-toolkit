using System.Runtime.CompilerServices;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

[AttributeUsage(AttributeTargets.Method)]
public class ExpressionAttribute([CallerFilePath] string sourceFilePath = "") : Attribute
{
    public string SourceFilePath { get; } = sourceFilePath;
}