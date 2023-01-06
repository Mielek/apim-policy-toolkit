using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class AuthenticationBasicPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new AuthenticationBasicPolicyHandler();
    protected override IPolicy Policy => new AuthenticationBasicPolicyBuilder()
        .Username("user@name.com")
        .Password("P4$$W0RD")
        .Build();
    protected override string Expected => @"<authentication-basic username=""user@name.com"" password=""P4$$W0RD"" />";
}