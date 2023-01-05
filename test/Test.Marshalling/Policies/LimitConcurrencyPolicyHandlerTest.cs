using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class LimitConcurrencyPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expected = @"<limit-concurrency key=""@(context.User.Id)"" max-count=""10""><base /></limit-concurrency>";
    readonly LimitConcurrencyPolicy _policy = new LimitConcurrencyPolicyBuilder()
            .Key(_ => _.Inlined(context => context.User.Id))
            .MaxCount(10)
            .Policies(_ => _.Base())
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new LimitConcurrencyPolicyHandler();

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