using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class WaitPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new WaitPolicyHandler();
    protected override IPolicy Policy => new WaitPolicyBuilder()
        .Policies(_ => _.Base())
        .Build();
    protected override string Expected => @"<wait><base /></wait>";
}