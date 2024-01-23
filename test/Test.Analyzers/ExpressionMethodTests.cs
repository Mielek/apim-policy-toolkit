using System.Data;

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
                    .Body((IContext c1) => "10")
                )
            )
            .Create();
    }
}
""",
            DiagnosticResult
                .CompilerError(Rules.Expression.WrongParameterName.Id)
                .WithSpan(16, 37, 16, 39)
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
                .WithSpan(16, 27, 16, 29)
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
                void Method(IContext context)
                {
                    return;
                }
            }
            """,
            DiagnosticResult
                .CompilerError(Rules.Expression.ReturnTypeNotAllowed.Id)
                .WithSpan(12, 5, 12, 9)
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
                .WithSpan(12, 15, 12, 15 + parameters.Length + 2)
                .WithArguments(count)
        );
    }
}
