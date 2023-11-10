using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mielek.Analyzers;
using Mielek.Analyzers.Test;
using System.Data;

namespace Mielek.Test.Analyzers;

[TestClass]
public class PolicyDocumentMethodTests
{
    public static Task VerifyAsync(string source, params DiagnosticResult[] diags)
    {
        return new BaseAnalyzerTest<PolicyDocumentMethodAnalyzer>(source, diags).RunAsync();
    }

    [TestMethod]
    public async Task ShouldReportWrongReturnType()
    {
        await VerifyAsync(
            """
            class Test 
            { 
                [Document]
                void Method() 
                { 
                    return;
                }
            }
            """,
            DiagnosticResult
                .CompilerError(Rules.PolicyDocument.ReturnValue.Id)
                .WithSpan(11, 5, 11, 9)
                .WithArguments(typeof(void).FullName)
        );
    }

    [TestMethod]
    [DataRow("IContext c1", 1)]
    [DataRow("IContext c1, IContext c2", 2)]
    public async Task ShouldReportNoParametersAllowed(string parameters, int count)
    {
        await VerifyAsync(
            $$"""
            class Test 
            { 
                [Document]
                PolicyDocument Method({{parameters}})
                { 
                    return null;
                }
            }
            """,
            DiagnosticResult
                .CompilerError(Rules.PolicyDocument.NoParametersAllowed.Id)
                .WithSpan(11, 26, 11, 26 + parameters.Length + 2)
                .WithArguments(count)
        );
    }
}
