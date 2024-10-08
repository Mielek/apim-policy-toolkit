using Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers;
using Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers.Test;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Test.Analyzers;

[TestClass]
public class ExpressionDefinitionTests
{
    public static Task VerifyAsync(string source, params DiagnosticResult[] diags)
    {
        return new BaseAnalyzerTest<ExpressionDefinitionAnalyzer>(source, diags).RunAsync();
    }

    [TestMethod]
    public async Task ShouldReportWrongParameterNameForParenthesisLambda()
    {
        await VerifyAsync(
"""
class Test
{
    void Method()
    {
        Policy.Document()
            .Inbound(policies => policies
                .SetBody(policy => policy
                    .Body((IExpressionContext c1) => "10")
                )
            )
            .Create();
    }
}
""",
            DiagnosticResult
                .CompilerError(Rules.Expression.WrongParameterName.Id)
                .WithSpan(15, 47, 15, 49)
                .WithArguments("c1", "context")
        );
    }

    [TestMethod]
    public async Task ShouldReportWrongParameterNameForSimpleLambda()
    {
        await VerifyAsync(
"""
class Test
{
    void Method()
    {
        Policy.Document()
            .Inbound(policies => policies
                .SetBody(policy => policy
                    .Body(c1 => "10")
                )
            )
            .Create();
    }
}
""",
            DiagnosticResult
                .CompilerError(Rules.Expression.WrongParameterName.Id)
                .WithSpan(15, 27, 15, 29)
                .WithArguments("c1", "context")
        );
    }

    [TestMethod]
    public async Task ShouldReportWrongReturnType()
    {
        await VerifyAsync(
            """
            class Test
            {
                [Expression]
                void Method(IExpressionContext context)
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
    [DataRow("IExpressionContext c1, IExpressionContext c2", 2)]
    [DataRow("IExpressionContext c1, IExpressionContext c2, IExpressionContext c3", 3)]
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