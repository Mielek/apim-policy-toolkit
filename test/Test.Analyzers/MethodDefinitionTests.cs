using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mielek.Analyzers;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Mielek.Model.Attributes;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Mielek.Analyzers.Test;
using System.Data;

namespace Mielek.Test.Analyzers;

using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<ExpressionMethodAnalyzer>;

[TestClass]
public class MethodDefinitionTests
{
    [TestMethod]
    public async Task ShouldReportWrongReturnType()
    {
        await ExpressionMethodAnalyzerTest.VerifyAsync(
            """
            class Test 
            { 
                [Expression]
                void Method(IContext context) 
                { 
                    return;
                }
            }
            """,
            DiagnosticResult
                .CompilerError(Rules.Expression.ReturnTypeNotAllowed.Id)
                .WithSpan(11, 5, 11, 9)
                .WithArguments(typeof(void).FullName)
        );
    }

    [TestMethod]
    [DataRow("", 0)]
    [DataRow("IContext c1, IContext c2", 2)]
    [DataRow("IContext c1, IContext c2, IContext c3", 3)]
    public async Task ShouldReportWrongWrongParameterCount(string parameters, int count)
    {
        await ExpressionMethodAnalyzerTest.VerifyAsync(
            $$"""
            class Test 
            { 
                [Expression]
                int Method({{parameters}})
                { 
                    return 10;
                }
            }
            """,
            DiagnosticResult
                .CompilerError(Rules.Expression.WrongParameterCount.Id)
                .WithSpan(11, 15, 11, 15 + parameters.Length + 2)
                .WithArguments(count)
        );
    }
}
