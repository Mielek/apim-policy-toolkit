
using System.Collections.Immutable;
using System.Reflection.Metadata;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

using Mielek.Builders;
using Mielek.Expressions.Context;
using Mielek.Model.Attributes;

namespace Mielek.Analyzers.Test;

public class ExpressionMethodAnalyzerTest : CSharpAnalyzerTest<ExpressionMethodAnalyzer, MSTestVerifier>
{

    ExpressionMethodAnalyzerTest(string source, params DiagnosticResult[] diags)
    {
        ReferenceAssemblies = new ReferenceAssemblies("net7.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "7.0.0"), Path.Combine("ref", "net7.0"));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(ExpressionAttribute).Assembly.Location));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(PolicyDocumentBuilder).Assembly.Location));
        TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(typeof(IContext).Assembly.Location));
        TestState.Sources.Add(
            $"""
            using Mielek.Builders;
            using Mielek.Expressions.Context;
            using Mielek.Model;
            using Mielek.Model.Attributes;

            namespace Mielek.Test;

            {source}
            """
        );
        TestState.ExpectedDiagnostics.AddRange(diags);

    }

    public static Task VerifyAsync(string source, params DiagnosticResult[] diags)
    {
        return new ExpressionMethodAnalyzerTest(source, diags).RunAsync();
    }


}