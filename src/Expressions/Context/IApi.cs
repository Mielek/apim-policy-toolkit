namespace Mielek.Expressions.Context;
public interface IApi
{
    string Id { get; }
    string Name { get; }
    string Path { get; }
    IEnumerable<string> Protocols { get; }
    IUrl ServiceUrl { get; }
    ISubscriptionKeyParameterNames SubscriptionKeyParameterNames { get; }
}