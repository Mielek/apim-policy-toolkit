using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class QuotaPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<quota renewal-period=""10"" />";
    readonly QuotaPolicy _policy = new QuotaPolicyBuilder()
        .RenewalPeriod(10)
        .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new QuotaPolicyHandler();

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