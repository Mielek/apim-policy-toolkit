using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SetMethodPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new SetMethodPolicyHandler();
    protected override IPolicy Policy => new SetMethodPolicyBuilder().Get().Build();
    protected override string Expected => "<set-method>GET</set-method>";
}