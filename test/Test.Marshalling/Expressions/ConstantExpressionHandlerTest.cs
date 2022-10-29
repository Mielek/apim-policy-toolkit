using System.Xml;

using Mielek.Marshalling;
using Mielek.Marshalling.Expressions;
using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ConstantExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallConstantExpression()
    {
        var handler = new ConstantExpressionHandler();

        handler.Marshal(Marshaller, new ConstantExpression("TestConstant"));

        Assert.AreEqual("TestConstant", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        new ConstantExpression("RegisterTest").Accept(Marshaller);

        Assert.AreEqual("RegisterTest", WrittenText.ToString());
    }
}