using Mielek.Expressions.Context;

namespace Mielek.Expressions.Testing.Mocks;

public class MockApi : IApi
{
    public string Id => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

    public string Path => throw new NotImplementedException();

    public IEnumerable<string> Protocols => throw new NotImplementedException();

    public IUrl ServiceUrl => throw new NotImplementedException();

    public ISubscriptionKeyParameterNames SubscriptionKeyParameterNames => throw new NotImplementedException();
}