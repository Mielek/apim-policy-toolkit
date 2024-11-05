using System.Diagnostics.Contracts;

using Azure.ApiManagement.PolicyToolkit.Compilation;

namespace Azure.ApiManagement.PolicyToolkit.Assertions;

public static class AssertionExtensions
{
    [Pure]
    public static CompilationResultAssertion Should(this ICompilationResult compilationResult)
    {
        return new CompilationResultAssertion(compilationResult);
    }
}