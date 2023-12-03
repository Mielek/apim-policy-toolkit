namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;
public interface IUserIdentity
{
    string Id { get; }
    string Provider { get; }
}