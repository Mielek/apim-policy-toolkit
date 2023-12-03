namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;
public interface IResponse
{
    IMessageBody Body { get; }
    IReadOnlyDictionary<string, string[]> Headers { get; }
    int StatusCode { get; }
    string StatusReason { get; }
}