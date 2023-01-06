using Mielek.Marshalling;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public abstract class BaseHandlerTest : BaseMarshallerTest
{
    protected abstract IMarshallerHandler Handler { get; }
    protected abstract IPolicy Policy { get; }
    protected abstract string Expected { get; }

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        Handler.Marshal(Marshaller, Policy);

        Assert.AreEqual(Expected, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        Policy.Accept(Marshaller);

        Assert.AreEqual(Expected, WrittenText.ToString());
    }
}