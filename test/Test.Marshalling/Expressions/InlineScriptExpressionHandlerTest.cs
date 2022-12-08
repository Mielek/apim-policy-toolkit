using Mielek.Builders.Expressions;
using Mielek.Marshalling.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class InlineScriptExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallObject()
    {
        var handler = new InlineScriptExpressionHandler();

        handler.Marshal(Marshaller, ExpressionBuilder.Builder.Inlined(context => context.Deployment.Region).Build());

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        var expression = ExpressionBuilder.Builder.Inlined(context => context.Deployment.Region).Build();
        
        expression.Accept(Marshaller);

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }
}