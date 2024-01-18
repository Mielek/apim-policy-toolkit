using Contoso.Apis;

namespace Contoso.Test.Apis;

[TestClass]
public class EchoApiBuildTests
{
    [TestMethod]
    public void ShouldBuildCorrectly()
    {
        new EchoApi().RetrieveResource();
    }
}