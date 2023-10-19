using Mielek.Builders.Expressions;
using Mielek.Expressions.Context;
using Mielek.Marshalling.Expressions;
using Mielek.Model.Attributes;

namespace Mielek.Test.Marshalling;

[TestClass]
public class MethodExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallObject()
    {
        var handler = new MethodExpressionHandler<string>();

        handler.Marshal(Marshaller, ExpressionBuilder<string>.Builder.Method(TestMethod).Build());

        Assert.AreEqual("@{return context.Deployment.Region;}", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        var expression = ExpressionBuilder<string>.Builder.Method(TestMethod).Build();

        expression.Accept(Marshaller);

        Assert.AreEqual("@{return context.Deployment.Region;}", WrittenText.ToString());
    }


    [Expression]
    static string TestMethod(IContext context)
    {
        return context.Deployment.Region;
    }
}


