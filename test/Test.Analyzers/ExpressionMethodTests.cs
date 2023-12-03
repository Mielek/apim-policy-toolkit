using System.Data;

using Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers;
using Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers.Test;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Test.Analyzers;

[TestClass]
public class ExpressionMethodTests
{
    public static Task VerifyAsync(string source, params DiagnosticResult[] diags)
    {
        return new BaseAnalyzerTest<ExpressionMethodAnalyzer>(source, diags).RunAsync();
    }

    [TestMethod]
    public async Task ShouldReportWrongReturnType()
    {
        await VerifyAsync(
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
    public async Task ShouldReportWrongParameterCount(string parameters, int count)
    {
        await VerifyAsync(
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
