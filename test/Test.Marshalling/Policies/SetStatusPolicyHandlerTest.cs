using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SetStatusPolicyHandlerTest : BaseMarshallerTest
{
    private readonly string _expected = "<set-status code=\"204\" reason=\"No content\" />";
    private readonly SetStatusPolicy _policy = new SetStatusPolicyBuilder().Code(204).Reason("No content").Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new SetStatusPolicyHandler();

        handler.Marshal(Marshaller, _policy);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        _policy.Accept(Marshaller);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallPolicyWithoutReasonField()
    {
        var policy = new SetStatusPolicyBuilder().Code(204).Build();

        policy.Accept(Marshaller);

        Assert.AreEqual("<set-status code=\"204\" />", WrittenText.ToString());
    }
}