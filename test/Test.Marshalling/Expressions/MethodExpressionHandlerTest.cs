using Mielek.Builders.Expressions;
using Mielek.Expressions.Context;
using Mielek.Marshalling.Expressions;
using Mielek.Model.Attributes;

namespace Mielek.Test.Marshalling;

[TestClass]
public class MethodExpressionHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        var expression = ExpressionBuilder<string>.Builder.Method(TestBodyMethod).Build();

        expression.Accept(Marshaller);

        Assert.AreEqual("@{return \"TestBodyMethod\";}", WrittenText.ToString());
    }

    [TestMethod]
    [DynamicData(nameof(BodyMethods), DynamicDataSourceType.Method)]
    public void ShouldMarshallBodyMethod(Func<IContext, string> func, string expected)
    {
        var handler = new MethodExpressionHandler<string>();

        handler.Marshal(Marshaller, ExpressionBuilder<string>.Builder.Method(func).Build());

        Assert.AreEqual($"@{{return \"{expected}\";}}", WrittenText.ToString());
    }

    [TestMethod]
    [DynamicData(nameof(ExpressionBodyMethods), DynamicDataSourceType.Method)]
    public void ShouldMarshallExpressionBodyMethod(Func<IContext, string> func, string expected)
    {
        var handler = new MethodExpressionHandler<string>();

        handler.Marshal(Marshaller, ExpressionBuilder<string>.Builder.Method(func).Build());

        Assert.AreEqual($"@(\"{expected}\")", WrittenText.ToString());
    }

    public static IEnumerable<object[]> BodyMethods() {
        yield return new object[] { (Func<IContext, string>)new MethodExpressionHandlerTest().TestBodyMethod, nameof(TestBodyMethod) };
        yield return new object[] { (Func<IContext, string>)StaticExpressionLibrary.StaticBodyMethod, nameof(StaticExpressionLibrary.StaticBodyMethod) };
        yield return new object[] { (Func<IContext, string>)new NonStaticExpressionLibrary().BodyMethod, nameof(NonStaticExpressionLibrary.BodyMethod) };
    }

    public static IEnumerable<object[]> ExpressionBodyMethods() {
        yield return new object[] { (Func<IContext, string>)new MethodExpressionHandlerTest().TestExpressionMethod, nameof(TestExpressionMethod) };
        yield return new object[] { (Func<IContext, string>)StaticExpressionLibrary.StaticExpressionBodyMethod, nameof(StaticExpressionLibrary.StaticExpressionBodyMethod) };
        yield return new object[] { (Func<IContext, string>)new NonStaticExpressionLibrary().ExpressionBodyMethod, nameof(NonStaticExpressionLibrary.ExpressionBodyMethod) };
    }

    string TestExpressionMethod(IContext context) => "TestExpressionMethod";

    string TestBodyMethod(IContext context)
    {
        return "TestBodyMethod";
    }
}


