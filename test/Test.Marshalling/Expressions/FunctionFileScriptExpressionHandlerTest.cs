using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class FunctionFileScriptExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallObject()
    {
        new FunctionFileScriptExpression<string>(TestScripts.Functions, "FunctionExpression").Accept(Marshaller);

        Assert.AreEqual("@(context.Elapsed)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallFunctionBodyOneLine()
    {
        new FunctionFileScriptExpression<bool>(TestScripts.Functions, "FunctionBodyOneLine").Accept(Marshaller);

        Assert.AreEqual("@(context.RequestId != null)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallFunctionBodyMultiline()
    {
        new FunctionFileScriptExpression<string>(TestScripts.Functions, "FunctionBodyMultiline").Accept(Marshaller);

        var expected = @"@{
var response = context.Response.Body.As&lt;JObject&gt;();
    foreach (var key in new[] { ""current"", ""minutely"", ""hourly"", ""daily"", ""alerts"" })
    {
        response.Property(key)?.Remove();
    };
    return response.ToString();
}";
        Assert.AreEqual(expected, WrittenText.ToString());
    }
}