using Mielek.Expressions.Context;

namespace Mielek.Testing.Expressions.Mocks;

public class MockContext : IContext
{
    public MockContext()
    {
        RequestId = Guid.NewGuid();
        Timestamp = DateTime.Now;
        Elapsed = TimeSpan.Zero;
        Tracing = false;
        MockVariables = new Dictionary<string, object>();
        MockApi = new MockContextApi();
        MockRequest = new MockRequest();
        MockResponse = new MockResponse();
        MockSubscription = new MockSubscription();
        MockUser = new MockUser();
    }

    public Guid RequestId { get; set; }
    public DateTime Timestamp { get; set; }
    public TimeSpan Elapsed { get; set; }
    public bool Tracing { get; set; }

    public Dictionary<string, object> MockVariables { get; init; }
    public IReadOnlyDictionary<string, object> Variables => MockVariables;

    public MockContextApi MockApi { get; init; }
    public IContextApi Api => MockApi;

    public MockRequest MockRequest { get; init; }
    public IRequest Request => MockRequest;

    public MockResponse MockResponse { get; init; }
    public IResponse Response => MockResponse;


    public MockSubscription MockSubscription { get; init; }
    public ISubscription Subscription => MockSubscription;

    public MockUser MockUser { get; init; }
    public IUser User => MockUser;

    public IDeployment Deployment => throw new NotImplementedException();

    public ILastError LastError => throw new NotImplementedException();

    public IOperation Operation => throw new NotImplementedException();

    public IProduct Product => throw new NotImplementedException();

    public void Trace(string message)
    {
        throw new NotImplementedException();
    }
}