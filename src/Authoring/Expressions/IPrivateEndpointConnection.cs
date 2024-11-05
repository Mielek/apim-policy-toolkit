namespace Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
public interface IPrivateEndpointConnection
{
    string Name { get; }
    string GroupId { get; }
    string MemberName { get; }
}