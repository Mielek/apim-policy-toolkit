using Contoso.Apis;

using Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

using Newtonsoft.Json.Linq;

namespace Contoso.Test.Apis;

[TestClass]
public class ApiOperationPolicyTest
{
    [TestMethod]
    public void FilterSecrets()
    {
        var context = new MockExpressionContext();
        context.Response.Body.Content =
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