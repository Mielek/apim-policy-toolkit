using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ValidateAzureAdTokenPolicyHandlerTest : BaseMarshallerTest
{
    private readonly string _expected = "<validate-azure-ad-token><client-application-ids><application-id>Client</application-id></client-application-ids></validate-azure-ad-token>";
    private readonly ValidateAzureAdTokenPolicy _policy = new ValidateAzureAdTokenPolicyBuilder()
        .ClientApplicationId("Client")
        .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new ValidateAzureAdTokenPolicyHandler();

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