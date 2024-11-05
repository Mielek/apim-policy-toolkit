using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Analyzers.Test;

public class BaseAnalyzerTest<TAnalyzer> : CSharpAnalyzerTest<TAnalyzer, MSTestVerifier> where TAnalyzer : DiagnosticAnalyzer, new()
{
    public BaseAnalyzerTest(string source, params DiagnosticResult[] diags)
    {
        ReferenceAssemblies = new ReferenceAssemblies("net8.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "8.0.0"), Path.Combine("ref", "net8.0"));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(ExpressionAttribute).Assembly.Location));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(Expression<>).Assembly.Location));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(IExpressionContext).Assembly.Location));
        TestState.Sources.Add(
            $"""
            using Azure.ApiManagement.PolicyToolkit.Authoring;
            using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

            namespace Mielek.Test;

            {source}
            """
        );
        TestState.ExpectedDiagnostics.AddRange(diags);

    }
}