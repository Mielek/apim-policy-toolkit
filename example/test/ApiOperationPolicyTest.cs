using Contoso.Apis;

using Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

using Newtonsoft.Json.Linq;

namespace Contoso.Test.Apis;

[TestClass]
public class ApiOperationPolicyTest
{
    [TestMethod]
    public void FilterSecrets()
    {
        var context = new MockContext();
        context.MockResponse.MockBody.Content =
            """
            {
                "title": "Software Engineer",
                "location": "Redmond",
                "secret": "42",
                "name": "John Doe"
            }
            """;

        var newBody = new ApiOperationPolicy().FilterSecrets(context);

        var expected = """
                       {
                           "title": "Software Engineer",
                           "name": "John Doe"
                       }
                       """;
        Assert.IsTrue(
            JObject.DeepEquals(
                JObject.Parse(newBody),
                JObject.Parse(expected)
            )
        );
    }
}