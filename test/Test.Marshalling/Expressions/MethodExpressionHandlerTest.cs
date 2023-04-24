using Mielek.Builders.Expressions;
using Mielek.Expressions.Context;
using Mielek.Marshalling.Expressions;
using Mielek.Model.Expressions;

namespace Mielek.Test.Marshalling;

[TestClass]
public class MethodExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallLambdaObject()
    {
        Marshaller.Options.MethodLibrary["lambda"] = "lambdaCode";
        var handler = new MethodExpressionHandler<string>();

        handler.Marshal(Marshaller, ExpressionBuilder<string>.Builder.FromMethod([LambdaExpression("lambda")] (context) => context.Deployment.Region).Build());
        

        Assert.AreEqual("@(lambdaCode)", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallMethodObject()
    {
        Marshaller.Options.MethodLibrary["TestMethod"] = "methodCode";
        var handler = new MethodExpressionHandler<string>();

        handler.Marshal(Marshaller, ExpressionBuilder<string>.Builder.FromMethod(TestMethod).Build());

        Assert.AreEqual("@(methodCode)", WrittenText.ToString());
    }


    [MethodExpression]
    static string TestMethod(IContext context)
    {
        return "";
    }
}


