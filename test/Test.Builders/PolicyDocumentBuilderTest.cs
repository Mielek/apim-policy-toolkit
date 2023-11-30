using Mielek.Builders;
using Mielek.Model.Attributes;

namespace Builders;

[TestClass]
public class PolicyDocumentBuilderTest
{
    [TestMethod]
    public void TestMethod1()
    {
        throw new Exception(PolicyDocumentBuilder.Create()
        .Inbound(s => {})
        .Build().ToString());
    }
}