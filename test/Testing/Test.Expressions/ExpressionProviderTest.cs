using Mielek.Testing.Expressions;
using Mielek.Testing.Expressions.Mocks;

namespace Mielek.Test.Testing.Expressions;

[TestClass]
public class ExpressionProviderTest
{
    [TestMethod]
    public async Task ShouldProvideExpressionFromFile()
    {
        var expression = ExpressionProvider.LoadFromFile<string>(TestScripts.WithoutDirective);

        var context = new MockContext();
        var expressionResult = await expression.Execute(context);

        Assert.AreEqual(context.RequestId.ToString(), expressionResult);
    }

    [TestMethod]
    public async Task ShouldProvideExpressionFromFileAndRemoveDirective()
    {
        var expression = ExpressionProvider.LoadFromFile<string>(TestScripts.WithDirective);

        var context = new MockContext();
        var expressionResult = await expression.Execute(context);

        Assert.AreEqual(context.RequestId.ToString(), expressionResult);
    }

    [DataTestMethod]
    [DataRow("FunctionBodyOneLine")]
    [DataRow("FunctionBodyMultiline")]
    [DataRow("FunctionExpression")]
    public async Task ShouldProvideExpressionFromFunctionFile(string functionName)
    {
        var expression = ExpressionProvider.LoadFromFunctionFile(TestScripts.Functions)[functionName];

        var context = new MockContext();
        var expressionResult = await expression.Execute(context);

        Assert.IsNotNull(expressionResult);
        Assert.AreEqual(context.RequestId.ToString(), expressionResult.ToString());
    }
}