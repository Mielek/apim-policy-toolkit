using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class ReturnResponsePolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new ReturnResponsePolicyHandler();
    protected override IPolicy Policy => new ReturnResponsePolicyBuilder()
        .Build();
    protected override string Expected => @"<return-response />";
}