namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
public interface IApi
{
    string Id { get; }
    string Name { get; }
    string Path { get; }
    IEnumerable<string> Protocols { get; }
    IUrl ServiceUrl { get; }
    ISubscriptionKeyParameterNames SubscriptionKeyParameterNames { get; }
}