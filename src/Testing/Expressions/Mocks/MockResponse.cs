using Mielek.Expressions.Context;

namespace Mielek.Testing.Expressions.Mocks;

public class MockResponse : IResponse
{
    public MockResponse()
    {
        MockBody = new MockBody();
    }


    public MockBody MockBody { get; init; }
    public IMessageBody Body => MockBody;

    public IReadOnlyDictionary<string, string[]> Headers => throw new NotImplementedException();

    public int StatusCode => throw new NotImplementedException();

    public string StatusReason => throw new NotImplementedException();
}