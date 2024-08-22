using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

[TestClass]
public class PolicyDocumentBuilderTest
{
    [TestMethod]
    public void TestMethod1()
    {
        var test = Policy.Document().Inbound(i => i.SetBody(p => p.Body([Expression] (context) => "test"))).Create();

        Console.Write(test.Name);
    }
}