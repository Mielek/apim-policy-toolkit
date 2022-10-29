using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SetHeaderPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<set-header name=""X-Test"" exist-action=""override""><value>3</value><value>2</value><value>1</value></set-header>";
    readonly SetHeaderPolicy _policy = new SetHeaderPolicyBuilder().Name("X-Test").ExistAction(ExistAction.Override).Value("3").Value("2").Value("1").Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new SetHeaderPolicyHandler();

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
    public void ShouldMarshallPolicyWithoutExistActionField()
    {
        var policy = new SetHeaderPolicyBuilder().Name("X-Test").Value("1").Build();

        policy.Accept(Marshaller);

        Assert.AreEqual(@"<set-header name=""X-Test""><value>1</value></set-header>", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallPolicyWithoutAnyValue()
    {
        var policy = new SetHeaderPolicyBuilder().Name("X-Test").ExistAction(ExistAction.Delete).Build();

        policy.Accept(Marshaller);

        Assert.AreEqual(@"<set-header name=""X-Test"" exist-action=""delete"" />", WrittenText.ToString());
    }
}