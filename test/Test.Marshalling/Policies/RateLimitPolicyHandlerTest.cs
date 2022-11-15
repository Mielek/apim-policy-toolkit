using System.Xml;

using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Expressions;
using Mielek.Marshalling.Policies;
using Mielek.Model.Expressions;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class RateLimitPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<rate-limit calls=""10"" renewal-period=""100"" />";
    readonly RateLimitPolicy _policy = new RateLimitPolicyBuilder()
            .Calls(10)
            .RenewalPeriod(100)
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new RateLimitPolicyHandler();

        handler.Marshal(Marshaller, _policy);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        _policy.Accept(Marshaller);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }
}