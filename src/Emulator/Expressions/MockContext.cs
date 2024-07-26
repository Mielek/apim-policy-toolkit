using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockContext : IContext
{
    public Guid RequestId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;
    public bool Tracing { get; set; } = false;

    public Dictionary<string, object> MockVariables { get; set; } = new Dictionary<string, object>();
    public IReadOnlyDictionary<string, object> Variables => MockVariables;

    public MockContextApi MockApi { get; set; } = new MockContextApi();
    public IContextApi Api => MockApi;

    public MockRequest MockRequest { get; set; } = new MockRequest();
    public IRequest Request => MockRequest;

    public MockResponse MockResponse { get; set; } = new MockResponse();
    public IResponse Response => MockResponse;


    public MockSubscription MockSubscription { get; set; } = new MockSubscription();
    public ISubscription Subscription => MockSubscription;

    public MockUser MockUser { get; set; } = new MockUser();
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