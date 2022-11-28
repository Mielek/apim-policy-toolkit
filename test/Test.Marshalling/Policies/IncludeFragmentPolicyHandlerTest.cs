using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class IncludeFragmentPolicyHandlerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new IncludeFragmentPolicyHandler();

        handler.Marshal(Marshaller, new IncludeFragmentPolicy("some-policy-id"));

        Assert.AreEqual("<include-fragment fragment-id=\"some-policy-id\" />", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        new IncludeFragmentPolicy("some-policy-id").Accept(Marshaller);

        Assert.AreEqual("<include-fragment fragment-id=\"some-policy-id\" />", WrittenText.ToString());
    }
}