using System.Diagnostics.Contracts;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Assertions;

public static class AssertionExtensions
{
    [Pure]
    public static CompilationResultAssertion Should(this ICompilationResult compilationResult)
    {
        return new CompilationResultAssertion(compilationResult);
    }
}