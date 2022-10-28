namespace Mielek.Expressions.Context;
public interface IContext
{
    Guid RequestId { get; }
    DateTime Timestamp { get; }
    TimeSpan Elapsed { get; }
    bool Tracing { get; }
    IReadOnlyDictionary<string, object> Variables { get; }
    IContextApi Api { get; }
    IRequest Request { get; }
    IResponse Response { get; }
    ISubscription Subscription { get; }
    IUser User { get; }
}
