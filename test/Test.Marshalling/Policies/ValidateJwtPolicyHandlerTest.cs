using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ValidateJwtPolicyHandlerTest : BaseMarshallerTest
{
    private readonly string _expected = "<validate-jwt />";
    private readonly ValidateJwtPolicy _policy = new ValidateJwtPolicy();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new ValidateJwtPolicyHandler();

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