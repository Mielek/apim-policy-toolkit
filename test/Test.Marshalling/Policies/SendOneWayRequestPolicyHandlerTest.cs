using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SendOneWayRequestPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new SendOneWayRequestPolicyHandler();
    protected override IPolicy Policy => new SendOneWayRequestPolicyBuilder()
        .SetUrl("https://test.com/")
        .SetMethod("POST")
        .SetBody("Body-body")
        .Build();
    protected override string Expected => @"<send-one-way-request><set-url>https://test.com/</set-url><set-method>POST</set-method><set-body>Body-body</set-body></send-one-way-request>";
}