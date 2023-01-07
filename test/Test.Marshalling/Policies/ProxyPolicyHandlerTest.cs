using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ProxyPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new ProxyPolicyHandler();
    protected override IPolicy Policy => new ProxyPolicyBuilder()
        .Url("https://test.com/")
        .Build();
    protected override string Expected => @"<proxy url=""https://test.com/"" />";
}