namespace Mielek.Expressions.Context;
public interface IUserIdentity
{
    string Id { get; }
    string Provider { get; }
}