using Mielek.Builders.Policies;
using Mielek.Marshalling;
using Mielek.Marshalling.Policies;
using Mielek.Model.Policies;

namespace Mielek.Test.Marshalling;

[TestClass]
public class CacheStorePolicyHandlerTest : BaseHandlerTest
{
    protected override IMarshallerHandler Handler => new CacheStorePolicyHandler();
    protected override IPolicy Policy => new CacheStorePolicyBuilder()
        .Duration(100)
        .Build();
    protected override string Expected => @"<cache-store duration=""100"" />";
}