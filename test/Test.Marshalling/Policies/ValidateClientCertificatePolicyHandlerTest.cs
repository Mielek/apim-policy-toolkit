using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ValidateClientCertificatePolicyHandlerTest : BaseMarshallerTest
{
    private readonly string _expected = "<validate-client-certificate><identities /></validate-client-certificate>";
    private readonly ValidateClientCertificatePolicy _policy = new ValidateClientCertificatePolicy(new List<ValidateClientCertificateIdentity>());

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new ValidateClientCertificatePolicyHandler();

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