using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class AuthenticationCertificatePolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new AuthenticationCertificatePolicyHandler();
    protected override IPolicy Policy => new AuthenticationCertificatePolicyBuilder()
        .Thumbprint("CA06F56B258B7A0D4F2B05470939478651151984")
        .Build();
    protected override string Expected => @"<authentication-certificate thumbprint=""CA06F56B258B7A0D4F2B05470939478651151984"" />";
}