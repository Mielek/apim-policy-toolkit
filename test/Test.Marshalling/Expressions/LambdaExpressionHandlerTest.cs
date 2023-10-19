using Mielek.Builders.Expressions;
using Mielek.Expressions.Context;
using Mielek.Marshalling.Expressions;
using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class LambdaExpressionHandlerTest : BaseMarshallerTest
{

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        var expression = ExpressionBuilder<string>.Builder.Lambda(context => context.Deployment.Region).Build();

        expression.Accept(Marshaller);

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallOneLineLambda()
    {
        var handler = new LambdaExpressionHandler<string>();

        handler.Marshal(Marshaller, ExpressionBuilder<string>.Builder.Lambda(context => context.Deployment.Region).Build());

        Assert.AreEqual("@(context.Deployment.Region)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallMultiLineLambda()
    {
        var expression = ExpressionBuilder<string>.Builder.Lambda(context =>
        {
            return context.Deployment.Region;
        }).Build();

        expression.Accept(Marshaller);

        Assert.AreEqual("@{return context.Deployment.Region;}", WrittenText.ToString());
    }
}


