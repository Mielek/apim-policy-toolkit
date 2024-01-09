using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockApi : IApi
{
    public string Id { get; set; } = "CVjSPi5XPE";
    public string Name { get; set; } = "mock-api";

    public string Path { get; set; } = "/mock";

    public IEnumerable<string> Protocols { get; set; } = new[] { "https" };

    public IUrl ServiceUrl => MockServiceUrl;
    public MockUrl MockServiceUrl { get; set; } = new MockUrl();

    public ISubscriptionKeyParameterNames SubscriptionKeyParameterNames => MockSubscriptionKeyParameterNames;
    public MockSubscriptionKeyParameterNames MockSubscriptionKeyParameterNames { get; set; } = new MockSubscriptionKeyParameterNames();
}