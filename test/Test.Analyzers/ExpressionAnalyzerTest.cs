using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers.Test;

public class BaseAnalyzerTest<TAnalyzer> : CSharpAnalyzerTest<TAnalyzer, MSTestVerifier> where TAnalyzer : DiagnosticAnalyzer, new()
{
    public BaseAnalyzerTest(string source, params DiagnosticResult[] diags)
    {
        ReferenceAssemblies = new ReferenceAssemblies("net8.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "8.0.0"), Path.Combine("ref", "net8.0"));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(ExpressionAttribute).Assembly.Location));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(Expression<>).Assembly.Location));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(PolicyDocumentBuilder).Assembly.Location));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(IContext).Assembly.Location));
        TestState.Sources.Add(
            $"""
            using System.Xml.Linq;
            using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;
            using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
            using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;
            using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;

            namespace Mielek.Test;

            {source}
            """
        );
        TestState.ExpectedDiagnostics.AddRange(diags);

    }
}