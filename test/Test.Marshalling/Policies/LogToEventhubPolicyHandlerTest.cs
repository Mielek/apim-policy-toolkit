using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class LogToEventhubPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<log-to-eventhub logger-id=""some-logger-id"">@(context.User.Id)</log-to-eventhub>";
    readonly LogToEventhubPolicy _policy = new LogToEventhubPolicyBuilder()
            .LoggerId("some-logger-id")
            .Value(_ => _.Inlined(context => context.User.Id))
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new LogToEventhubPolicyHandler();

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