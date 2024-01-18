using System.Data;

using Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers;
using Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers.Test;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Test.Analyzers;

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
                XElement Method({{parameters}})
                { 
                    return null;
                }
            }
            """,
            DiagnosticResult
                .CompilerError(Rules.PolicyDocument.NoParametersAllowed.Id)
                .WithSpan(11, 20, 11, 20 + parameters.Length + 2)
                .WithArguments(count)
        );
    }
}