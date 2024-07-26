using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Serialization.Test;

[TestClass]
public class ExpressionBuilderTests
{
    [TestMethod]
    public void ShouldReturnConstantExpression()
    {
        var expression = ExpressionBuilder<string>.Builder.Constant("name").Build();
        Assert.AreEqual("name", expression.Source);
    }

    [TestMethod]
    public void ShouldReturnExpressionLambda()
    {
        var expression = ExpressionBuilder<string>.Builder.Lambda(context => context.Product.Name).Build();
        Assert.AreEqual("@(context.Product.Name)", expression.Source);
    }

    [TestMethod]
    public void ShouldReturnBodyLambda()
    {
        var expression = ExpressionBuilder<string>.Builder.Lambda(context =>
        {
            return context.Product.Name;
        }).Build();
        Assert.AreEqual("@{return context.Product.Name;}", expression.Source);
    }

    [TestMethod]
    public void ShouldReturnExpressionMethod()
    {
        var expression = ExpressionBuilder<string>.Builder.Method(ExpressionMethod).Build();
        Assert.AreEqual("@(context.Product.Name)", expression.Source);
    }

    [TestMethod]
    public void ShouldReturnBodyMethod()
    {
        var expression = ExpressionBuilder<string>.Builder.Method(BodyMethod).Build();
        Assert.AreEqual("@{return context.Product.Name;}", expression.Source);
    }


    [TestMethod]
    public void ShouldReturnExpressionLambdaForFunction()
    {
        var expression = ExpressionBuilder<string>.Builder.Function(context => context.Product.Name).Build();
        Assert.AreEqual("@(context.Product.Name)", expression.Source);
    }

    [TestMethod]
    public void ShouldReturnBodyLambdaForFunction()
    {
        var expression = ExpressionBuilder<string>.Builder.Function(context =>
        {
            return context.Product.Name;
        }).Build();
        Assert.AreEqual("@{return context.Product.Name;}", expression.Source);
    }

    [TestMethod]
    public void ShouldReturnExpressionMethodForFunction()
    {
        var expression = ExpressionBuilder<string>.Builder.Function(ExpressionMethod).Build();
        Assert.AreEqual("@(context.Product.Name)", expression.Source);
    }

    [TestMethod]
    public void ShouldReturnBodyMethodForFunction()
    {
        var expression = ExpressionBuilder<string>.Builder.Function(BodyMethod).Build();
        Assert.AreEqual("@{return context.Product.Name;}", expression.Source);
    }

    [Expression]
    public string ExpressionMethod(IContext context) => context.Product.Name;

    [Expression]
    public string BodyMethod(IContext context)
    {
        return context.Product.Name;
    }
}