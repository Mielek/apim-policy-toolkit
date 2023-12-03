using System.Security.Cryptography.X509Certificates;

using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockRequest : IRequest
{
    public MockRequest()
    {
        MockBody = new MockBody();
    }

    public MockBody MockBody { get; init; }
    public IMessageBody Body => MockBody;


    public X509Certificate2 Certificate => throw new NotImplementedException();

    public IReadOnlyDictionary<string, string[]> Headers => throw new NotImplementedException();

    public string IpAddress => throw new NotImplementedException();

    public IReadOnlyDictionary<string, string> MatchedParameters => throw new NotImplementedException();

    public string Method => throw new NotImplementedException();

    public IUrl OriginalUrl => throw new NotImplementedException();

    public IUrl Url => throw new NotImplementedException();

    public IPrivateEndpointConnection? PrivateEndpointConnection => throw new NotImplementedException();
}