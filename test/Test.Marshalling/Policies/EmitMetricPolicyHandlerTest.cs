using Mielek.Builders.Policies;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class EmitMetricPolicyHandlerTest : BaseMarshallerTest
{
    private readonly string _expected = @"<emit-metric name=""some-name""><dimension name=""dimension-name"" /></emit-metric>";
    private readonly EmitMetricPolicy _policy = new EmitMetricPolicyBuilder()
            .Name("some-name")
            .Dimension(_ => _.Name("dimension-name"))
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new EmitMetricPolicyHandler();

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