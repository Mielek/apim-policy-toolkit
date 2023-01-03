namespace Mielek.Test.Example;

using Mielek.Testing.Expressions;
using Mielek.Testing.Expressions.Mocks;

using static Constants;

[TestClass]
public class SimpleExpressionsLibraryTest
{
    static Dictionary<string, Expression<object>> _expression;

    [ClassInitialize]
    public static void Init(TestContext _)
    {
        _expression = ExpressionProvider.LoadFromFunctionFile(ExpressionPath("simple-expressions-library.csx"));
    }

    [ClassCleanup]
    public static void Cleanup()
    {
        _expression = null;
    }

    [DataTestMethod]
    [DataRow("IsVariableSet")]
    [DataRow("GetKnownGUIDOrGenerateNew")]
    public void ShouldContainMethod(string functionName)
    {
        Assert.IsNotNull(_expression[functionName]);
    }

    [TestMethod]
    public async Task ShouldReturnTrueWhenVariableIsSet()
    {
        var context = new MockContext();
        context.MockVariables["Variable"] = "something";

        var result = (bool) await _expression["IsVariableSet"].Execute(context);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ShouldReturnFalseWhenVariableIsNotSet()
    {
        var context = new MockContext();

        var result = (bool) await _expression["IsVariableSet"].Execute(context);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task ShouldProduceRandomGuid()
    {
        var expression = _expression["GetKnownGUIDOrGenerateNew"];
        var context = new MockContext();

        var guidOne = await expression.Execute(context);
        var guidTwo = await expression.Execute(context);

        Assert.AreNotEqual(guidOne, guidTwo);
    }

    [TestMethod]
    public async Task ShouldProduceKnownGuid()
    {
        var expression = _expression["GetKnownGUIDOrGenerateNew"];
        var knownGuid = Guid.NewGuid().ToString();
        var context = new MockContext();
        context.MockVariables["KnownGUID"] = knownGuid;

        var guidOne = await expression.Execute(context);
        var guidTwo = await expression.Execute(context);

        Assert.AreEqual(knownGuid, guidOne);
        Assert.AreEqual(guidOne, guidTwo);
    }
}