using Mielek.Builders.Expressions;
using Mielek.Marshalling.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class InlineExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallObject()
    {
        var handler = new InlineExpressionHandler<string>();

        handler.Marshal(Marshaller, ExpressionBuilder<string>.Builder.Inline(context => context.Deployment.Region).Build());

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        var expression = ExpressionBuilder<string>.Builder.Inline(context => context.Deployment.Region).Build();
        
        expression.Accept(Marshaller);

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }
}