using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class TracePolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new TracePolicyHandler();
    protected override IPolicy Policy => new TracePolicyBuilder()
        .Source("some-source")
        .Message("ohh message")
        .Build();
    protected override string Expected => @"<trace source=""some-source""><message>ohh message</message></trace>";
}