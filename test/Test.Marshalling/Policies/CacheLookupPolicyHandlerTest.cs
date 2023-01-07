using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class CacheLookupPolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new CacheLookupPolicyHandler();
    protected override IPolicy Policy => new CacheLookupPolicyBuilder()
        .VaryByDeveloper(true)
        .VaryByDeveloperGroup(false)
        .Build();
    protected override string Expected => @"<cache-lookup vary-by-developer=""True"" vary-by-developer-group=""False"" />";
}