using System.Text;
using System.Xml;

using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Test;

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