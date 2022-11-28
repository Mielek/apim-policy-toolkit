using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class QuotaByKeyPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<quota-by-key counter-key=""test"" renewal-period=""100"" calls=""10"" />";
    readonly QuotaByKeyPolicy _policy = new QuotaByKeyPolicyBuilder()
            .CounterKey("test")
            .RenewalPeriod(100)
            .Calls(10)
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new QuotaByKeyPolicyHandler();

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