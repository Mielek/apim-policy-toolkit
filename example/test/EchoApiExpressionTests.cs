
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;
using Contoso.Apis;
using Newtonsoft.Json.Linq;

namespace Contoso.Test.Apis;

[TestClass]
public class EchoApiExpressionTests
{
    [TestMethod]
    public void FilterBody()
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

        var newBody = new SimpleEchoApi().FilterBody(context);
        Assert.IsTrue(JObject.DeepEquals(JObject.Parse(newBody), JObject.Parse("""
                                                                               {
                                                                                   "title": "Software Engineer",
                                                                                   "name": "John Doe"
                                                                               }
                                                                               """)));
    }
}