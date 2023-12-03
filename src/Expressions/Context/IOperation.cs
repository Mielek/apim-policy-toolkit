namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;
public interface IOperation
{
    string Id { get; }
    string Method { get; }
    string Name { get; }
    string UrlTemplate { get; }
}