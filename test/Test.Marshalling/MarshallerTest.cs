using Mielek.Builders;
namespace Mielek.Test.Marshalling;

[TestClass]
public class MarshallerTest : BaseMarshallerTest
{
    [TestMethod]
    public void ShouldMarshallPolicyDocument()
    {
        var document = PolicyDocumentBuilder
            .Create()
            .Inbound(policies => policies.Base())
            .Backend(policies => policies.Base())
            .Outbound(policies => policies.Base())
            .OnError(policies => policies.Base())
            .Build();

        document.Accept(Marshaller);

        Assert.AreEqual("<policies><inbound><base /></inbound><backend><base /></backend><outbound><base /></outbound><on-error><base /></on-error></policies>", WrittenText);
    }

    [TestMethod]
    public void ShouldMarshallPolicyFragment()
    {
        var fragment = PolicyFragmentBuilder.Create()
            .Policies(policies => policies.SetMethod(policy => policy.Options()))
            .Build();

        fragment.Accept(Marshaller);

        Assert.AreEqual(@"<fragment><set-method method=""OPTIONS"" /></fragment>", WrittenText);
    }
}