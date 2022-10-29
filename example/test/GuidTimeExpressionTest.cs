namespace test;

using static Constants;

using Mielek.Expressions.Testing;
using Mielek.Expressions.Testing.Mocks;

[TestClass]
public class GuidTimeExpressionTest
{
    static Expression expression;

    [ClassInitialize]
    public static void Init(TestContext c)
    {
        expression = ExpressionProvider.LoadFromFile(ExpressionPath("guid-time.csx"));
    }

    [ClassCleanup]
    public static void Cleanup()
    {
        expression = null;
    }

    [TestMethod]
    public async Task ShouldBeGuid()
    {
        var result = await expression.Execute(new MockContext());

        Guid.Parse(result);
    }

    [TestMethod]
    public async Task ShouldProduceDifferentGuids()
    {
        var first = await expression.Execute(new MockContext());
        var second = await expression.Execute(new MockContext());

        Assert.AreNotEqual(first, second);
    }
}