using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class AuthenticationManagedIdentityPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new AuthenticationManagedIdentityPolicyHandler();
    protected override IPolicy Policy => new AuthenticationManagedIdentityPolicyBuilder()
        .Resource("https://graph.microsoft.com")
        .Build();
    protected override string Expected => @"<authentication-managed-identity resource=""https://graph.microsoft.com"" />";
}