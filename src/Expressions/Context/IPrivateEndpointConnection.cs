namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;
public interface IPrivateEndpointConnection
{
    string Name { get; }
    string GroupId { get; }
    string MemberName { get; }
}