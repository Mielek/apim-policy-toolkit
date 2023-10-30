using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ChoosePolicyHandlerTest : BaseMarshallerTest
{
    private readonly string _expected = @"<choose><when condition=""True""><base /></when></choose>";
    private readonly ChoosePolicy _policy = new ChoosePolicyBuilder()
            .When(_ => _
                .Condition(_ => _.Constant(true))
                .Policies(_ => _.Base())
            )
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new ChoosePolicyHandler();

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