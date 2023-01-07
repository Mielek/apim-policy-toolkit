using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SendRequestPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new SendRequestPolicyHandler();
    protected override IPolicy Policy => new SendRequestPolicyBuilder()
        .ResponseVariableName("res-var")
        .SetUrl("https://test.com/")
        .SetMethod(_ => _.Post())
        .SetBody("Body-body")
        .Build();
    protected override string Expected => @"<send-request response-variable-name=""res-var""><set-url>https://test.com/</set-url><set-method>POST</set-method><set-body>Body-body</set-body></send-request>";
}