using Mielek.Expressions.Context;

namespace Mielek.Testing.Expressions.Mocks;

public class MockApi : IApi
{
    public string Id { get; set; } = "abcdefgh";
    public string Name { get; set; } = "mock-api";

    public string Path { get; set; } = "/mock";

    public IEnumerable<string> Protocols { get; set; } = new[] { "https" };

    public IUrl ServiceUrl => MockServiceUrl;
    public MockUrl MockServiceUrl { get; set; } = new MockUrl();

    public ISubscriptionKeyParameterNames SubscriptionKeyParameterNames => MockSubscriptionKeyParameterNames;
    public MockSubscriptionKeyParameterNames MockSubscriptionKeyParameterNames { get; set; } = new MockSubscriptionKeyParameterNames();
}