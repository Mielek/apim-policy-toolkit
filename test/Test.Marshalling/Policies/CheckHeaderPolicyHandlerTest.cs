using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class CheckHeaderPolicyHandlerTest : BaseMarshallerTest
{
    private readonly string _expected = @"<check-header name=""amen"" failed-check-httpcode=""400"" failed-check-error-message=""MessageError"" ignore-case=""True""><value>3</value><value>2</value><value>1</value></check-header>";
    private readonly CheckHeaderPolicy _policy = new CheckHeaderPolicyBuilder()
            .Name("amen")
            .FailedCheckHttpCode(400)
            .FailedCheckErrorMessage("MessageError")
            .IgnoreCase(true)
            .Value("3")
            .Value("2")
            .Value("1")
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new CheckHeaderPolicyHandler();

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