using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockResponse : IResponse
{
    public MockBody Body { get; set; } = new MockBody();
    IMessageBody IResponse.Body => Body;

    public Dictionary<string, string[]> Headers { get; set; } = new Dictionary<string, string[]>();
    IReadOnlyDictionary<string, string[]> IResponse.Headers => Headers;

    public int StatusCode { get; set; } = 200;

    public string StatusReason { get; set; } = "OK";
}