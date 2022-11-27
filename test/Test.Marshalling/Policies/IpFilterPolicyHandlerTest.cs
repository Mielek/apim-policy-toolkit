using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class IpFilterPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<ip-filter action=""allow""><address>1.1.1.1</address><address-range from=""1.1.1.2"" to=""1.1.1.255"" /></ip-filter>";
    readonly IpFilterPolicy _policy = new IpFilterPolicyBuilder()
            .Action(IpFilterAction.Allow)
            .Address("1.1.1.1")
            .AddressRange("1.1.1.2", "1.1.1.255")
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new IpFilterPolicyHandler();

        handler.Marshal(Marshaller, _policy);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        _policy.Accept(Marshaller);

        Assert.AreEqual(_expected, WrittenText.ToString());
    }

    [DataTestMethod]
    [DataRow(IpFilterAction.Allow, "allow")]
    [DataRow(IpFilterAction.Forbid, "forbid")]
    public void ShouldMarshallAction(IpFilterAction action, string expected)
    {
        var handler = new IpFilterPolicyHandler();

        handler.Marshal(Marshaller, new IpFilterPolicyBuilder()
            .Action(action)
            .Address("1.1.1.1")
            .Build());

        Assert.AreEqual($@"<ip-filter action=""{expected}""><address>1.1.1.1</address></ip-filter>", WrittenText.ToString());
    }
}