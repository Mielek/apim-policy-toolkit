
using Mielek.Testing.Expressions.Mocks;
using Contoso.Apis;
using Newtonsoft.Json.Linq;

namespace Contoso.Test.Apis;

[TestClass]
public class EchoApiExpressionTests
{
    [TestMethod]
    public void FilterBody()
    {
        var context = new MockContext();
        context.MockResponse.MockBody.Content = "{ \"current\": \"some current content\", \"other\": \"some other content\" }";
        var echoApi = new EchoApi();

        var result = echoApi.FilterBody(context);

        var actual = JObject.Parse(result);
        var expected = JObject.Parse("{ \"other\": \"some other content\" }");

        Assert.IsTrue(JObject.DeepEquals(actual, expected));
    }

    [TestMethod]
    public void ShouldBeGuid()
    {
        var echoApi = new EchoApi();
        var result = echoApi.GetKnownGUIDOrGenerateNew(new MockContext());

        Guid.Parse(result);
    }

    [TestMethod]
    public void ShouldProduceDifferentGuids()
    {
        var echoApi = new EchoApi();
        var first = echoApi.GetKnownGUIDOrGenerateNew(new MockContext());
        var second = echoApi.GetKnownGUIDOrGenerateNew(new MockContext());

        Assert.AreNotEqual(first, second);
    }

    [TestMethod]
    public void ShouldProduceKnownGuid()
    {
        var echoApi = new EchoApi();
        var knownGuid = Guid.NewGuid().ToString();
        var context = new MockContext();
        context.MockVariables["KnownGUID"] = knownGuid;

        var guidOne = echoApi.GetKnownGUIDOrGenerateNew(context);
        var guidTwo = echoApi.GetKnownGUIDOrGenerateNew(context);

        Assert.AreEqual(knownGuid, guidOne);
        Assert.AreEqual(guidOne, guidTwo);
    }
}