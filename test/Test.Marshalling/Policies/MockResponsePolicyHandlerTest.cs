using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class MockResponsePolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new MockResponsePolicyHandler();
    protected override IPolicy Policy => new MockResponsePolicyBuilder().Build();
    protected override string Expected => @"<mock-response />";
}