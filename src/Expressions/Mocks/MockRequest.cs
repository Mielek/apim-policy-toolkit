using System.Security.Cryptography.X509Certificates;

using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockRequest : IRequest
{

    public MockBody MockBody { get; set; } = new MockBody();
    public IMessageBody Body => MockBody;


    public X509Certificate2? Certificate { get; set; } = null;

    public Dictionary<string, string[]> MockHeaders { get; set; } = new Dictionary<string, string[]>()
    {
        {"Accept", new [] {"application/json"}}
    };
    public IReadOnlyDictionary<string, string[]> Headers => throw new NotImplementedException();

    public string IpAddress { get; set; } = "192.168.0.1";

    public IReadOnlyDictionary<string, string> MatchedParameters => throw new NotImplementedException();

    public string Method => throw new NotImplementedException();

    public IUrl OriginalUrl => throw new NotImplementedException();

    public IUrl Url => throw new NotImplementedException();

    public IPrivateEndpointConnection? PrivateEndpointConnection => throw new NotImplementedException();
}