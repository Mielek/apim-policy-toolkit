using System.Xml;

using Mielek.Marshalling;
using Mielek.Marshalling.Expressions;
using Mielek.Model;
using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class FileScriptExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallObject()
    {
        var handler = new FileScriptExpressionHandler();

        handler.Marshal(Marshaller, new FileScriptExpression(TestScripts.OneLine));

        Assert.AreEqual($"@{{{Environment.NewLine}return context.RequestId;{Environment.NewLine}}}", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        new FileScriptExpression(TestScripts.OneLine).Accept(Marshaller);

        Assert.AreEqual($"@{{{Environment.NewLine}return context.RequestId;{Environment.NewLine}}}", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldRemoveDirectiveWhenMarshalling()
    {
        var handler = new FileScriptExpressionHandler();

        handler.Marshal(Marshaller, new FileScriptExpression(TestScripts.WithDirective));

        Assert.AreEqual($"@{{{Environment.NewLine}return context.User.LastName;{Environment.NewLine}}}", WrittenText.ToString());
    }
}