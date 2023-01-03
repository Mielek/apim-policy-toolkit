using Mielek.Marshalling.Expressions;
using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ConstantExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallConstantExpression()
    {
        var handler = new ConstantExpressionHandler<string>();

        handler.Marshal(Marshaller, new ConstantExpression<string>("TestConstant"));

        Assert.AreEqual("TestConstant", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        new ConstantExpression<string>("RegisterTest").Accept(Marshaller);

        Assert.AreEqual("RegisterTest", WrittenText.ToString());
    }
}