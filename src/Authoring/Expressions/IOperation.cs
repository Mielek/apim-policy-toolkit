namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
public interface IOperation
{
    string Id { get; }
    string Method { get; }
    string Name { get; }
    string UrlTemplate { get; }
}