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
        new FileScriptExpression<string>(TestScripts.OneLine).Accept(Marshaller);

        Assert.AreEqual($"@{{{Environment.NewLine}return context.RequestId;{Environment.NewLine}}}", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldRemoveDirectiveWhenMarshalling()
    {
        new FileScriptExpression<string>(TestScripts.WithDirective).Accept(Marshaller);

        Assert.AreEqual($"@{{{Environment.NewLine}return context.User.LastName;{Environment.NewLine}}}", WrittenText.ToString());
    }
}