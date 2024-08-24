namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public interface IExpressionContext
{
    IContextApi Api { get; }
    IDeployment Deployment { get; }
    TimeSpan Elapsed { get; }
    ILastError LastError { get; }
    IOperation Operation { get; }
    IProduct Product { get; }
    IRequest Request { get; }
    Guid RequestId { get; }
    IResponse Response { get; }
    ISubscription Subscription { get; }
    DateTime Timestamp { get; }
    bool Tracing { get; }
    IUser User { get; }
    IReadOnlyDictionary<string, object> Variables { get; }
    void Trace(string message);
}