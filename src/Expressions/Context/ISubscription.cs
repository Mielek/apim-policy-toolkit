namespace Mielek.Expressions.Context;
public interface ISubscription
{
    DateTime CreatedDate { get; }
    DateTime? EndDate { get; }
    string Id { get; }
    string Key { get; }
    string Name { get; }
    string PrimaryKey { get; }
    string SecondaryKey { get; }
    DateTime? StartDate { get; }
}