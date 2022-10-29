using System.Xml;

using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Expressions;
using Mielek.Marshalling.Policies;
using Mielek.Model.Expressions;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SetBodyPolicyHandlerTest : BaseMarshallerTest
{
    readonly string _expectedSimple = @"<set-body>SomeBody</set-body>";
    readonly SetBodyPolicy _simplePolicy = new SetBodyPolicyBuilder()
            .Body("SomeBody")
            .Build();

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new SetBodyPolicyHandler();

        handler.Marshal(Marshaller, _simplePolicy);

        Assert.AreEqual(_expectedSimple, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        _simplePolicy.Accept(Marshaller);

        Assert.AreEqual(_expectedSimple, WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldMarshallTemplate()
    {
        SetBodyPolicy policy = new SetBodyPolicyBuilder()
            .Body("SomeBody")
            .Template(BodyTemplate.Liquid)
            .Build();

        policy.Accept(Marshaller);

        Assert.AreEqual(@"<set-body template=""liquid"">SomeBody</set-body>", WrittenText.ToString());
    }


    [DataTestMethod]
    [DataRow(XsiNilType.Blank, "blank")]
    [DataRow(XsiNilType.Null, "null")]
    public void ShouldMarshallXsiNilType(XsiNilType xsiNilType, string expected)
    {
        SetBodyPolicy policy = new SetBodyPolicyBuilder()
            .Body("SomeBody")
            .XsiNil(xsiNilType)
            .Build();

        policy.Accept(Marshaller);

        Assert.AreEqual($"<set-body xsi-nil=\"{expected}\">SomeBody</set-body>", WrittenText.ToString());
    }

}