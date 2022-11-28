using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SetMethodPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = "<set-method method=\"GET\" />";
    readonly SetMethodPolicy _policy = new SetMethodPolicyBuilder().Get().Build();
    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new SetMethodPolicyHandler();

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