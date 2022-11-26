using System.Xml;

using Mielek.Marshalling;
using Mielek.Marshalling.Expressions;
using Mielek.Model;
using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class FunctionFileScriptExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallObject()
    {
        var handler = new FunctionFileScriptExpressionHandler();

        handler.Marshal(Marshaller, new FunctionFileScriptExpression(TestScripts.Functions, "FunctionExpression"));

        Assert.AreEqual("@(context.Elapsed)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        new FunctionFileScriptExpression(TestScripts.Functions, "FunctionExpression").Accept(Marshaller);

        Assert.AreEqual("@(context.Elapsed)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallFunctionBodyOneLine()
    {
        var handler = new FunctionFileScriptExpressionHandler();

        handler.Marshal(Marshaller, new FunctionFileScriptExpression(TestScripts.Functions, "FunctionBodyOneLine"));

        Assert.AreEqual("@(context.RequestId != null)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallFunctionBodyMultiline()
    {
        var handler = new FunctionFileScriptExpressionHandler();

        handler.Marshal(Marshaller, new FunctionFileScriptExpression(TestScripts.Functions, "FunctionBodyMultiline"));

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