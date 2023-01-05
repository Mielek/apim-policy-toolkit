using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ForwardRequestPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<forward-request />";
    readonly ForwardRequestPolicy _policy = new ForwardRequestPolicyBuilder()
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new ForwardRequestPolicyHandler();

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