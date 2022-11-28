using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class BasePolicyHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new BasePolicyHandler();

        handler.Marshal(Marshaller, new BasePolicy());

        Assert.AreEqual("<base />", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        new BasePolicy().Accept(Marshaller);

        Assert.AreEqual("<base />", WrittenText.ToString());
    }
}