using System.Xml;

using Mielek.Marshalling;
using Mielek.Marshalling.Expressions;
using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class InlineScriptExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallObject()
    {
        var handler = new InlineScriptExpressionHandler();

        handler.Marshal(Marshaller, new InlineScriptExpression("context.Deployment.Region"));

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        new InlineScriptExpression("context.Deployment.Region").Accept(Marshaller);

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }
}