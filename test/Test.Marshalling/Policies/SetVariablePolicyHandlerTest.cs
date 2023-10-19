using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class SetVariablePolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new SetVariablePolicyHandler();
    protected override IPolicy Policy => new SetVariablePolicyBuilder().Name("var-name").Value(_ => _.Inline(context => context.User.Id)).Build();
    protected override string Expected => @"<set-variable name=""var-name"" value=""@(context.User.Id)"" />";
}