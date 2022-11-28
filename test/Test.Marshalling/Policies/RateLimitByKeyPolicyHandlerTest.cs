using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class RateLimitByKeyPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<rate-limit-by-key calls=""10"" renewal-period=""100"" counter-key=""test"" />";
    readonly RateLimitByKeyPolicy _policy = new RateLimitByKeyPolicyBuilder()
            .Calls(10)
            .RenewalPeriod(100)
            .CounterKey("test")
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new RateLimitByKeyPolicyHandler();

        handler.Marshal(Marshaller, _policy);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        _policy.Accept(Marshaller);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }

    // TODO add test for rest properties

}