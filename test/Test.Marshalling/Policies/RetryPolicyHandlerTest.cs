using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class RetryPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new RetryPolicyHandler();
    protected override IPolicy Policy => new RetryPolicyBuilder()
        .Condition(_ => _.Inline(_ => _.User.Id == "admin"))
        .Count(10)
        .Interval(10)
        .Policies(_ => _.Base())
        .Build();
    protected override string Expected => @"<retry condition=""@((_.User.Id == ""admin""))"" count=""10"" interval=""10""><base /></retry>";
}