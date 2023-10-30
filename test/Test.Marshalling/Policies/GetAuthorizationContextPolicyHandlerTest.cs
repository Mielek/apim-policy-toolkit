using System.Xml;

using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Expressions;
using Mielek.Marshalling.Policies;
using Mielek.Model.Expressions;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class GetAuthorizationContextPolicyHandlerTest : BaseMarshallerTest
{
    private readonly GetAuthorizationContextPolicyBuilder _baseBuilder = new GetAuthorizationContextPolicyBuilder()
            .ProviderId("someProvider")
            .AuthorizationId("someAuthId")
            .ContextVariableName("someContextVar");

    [TestMethod]
    public void ShouldMarshallPolicy()
    {
        var handler = new GetAuthorizationContextPolicyHandler();
        var policy = _baseBuilder.Build();

        handler.Marshal(Marshaller, policy);

        Assert.AreEqual("<get-authorization-context provider-id=\"someProvider\" authorization-id=\"someAuthId\" context-variable=\"someContextVar\" />", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldHandlerBeRegisterInMarshaller()
    {
        var policy = _baseBuilder.Build();

        policy.Accept(Marshaller);

        Assert.AreEqual("<get-authorization-context provider-id=\"someProvider\" authorization-id=\"someAuthId\" context-variable=\"someContextVar\" />", WrittenText.ToString());
    }

    [DataTestMethod]
    [DataRow(IdentityType.JWT, "jwt")]
    [DataRow(IdentityType.Managed, "managed")]
    public void ShouldParseIdentityType(IdentityType identityType, string marshalledValue)
    {
        var policy = _baseBuilder
            .IdentityType(identityType)
            .Build();

        policy.Accept(Marshaller);

        Assert.AreEqual($"<get-authorization-context provider-id=\"someProvider\" authorization-id=\"someAuthId\" context-variable=\"someContextVar\" identity-type=\"{marshalledValue}\" />", WrittenText.ToString());
    }

    [TestMethod]
    public void ShouldParseIdentityProperty()
    {
        var policy = _baseBuilder
            .Identity("someIdentity")
            .Build();

        policy.Accept(Marshaller);

        Assert.AreEqual("<get-authorization-context provider-id=\"someProvider\" authorization-id=\"someAuthId\" context-variable=\"someContextVar\" identity=\"someIdentity\" />", WrittenText.ToString());
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void ShouldParseIgnoreProperty(bool ignoreError)
    {
        var policy = _baseBuilder
            .IgnoreError(ignoreError)
            .Build();

        policy.Accept(Marshaller);

        Assert.AreEqual($"<get-authorization-context provider-id=\"someProvider\" authorization-id=\"someAuthId\" context-variable=\"someContextVar\" ignore-error=\"{ignoreError}\" />", WrittenText.ToString());
    }
}