namespace Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
public interface IUserIdentity
{
    string Id { get; }
    string Provider { get; }
}