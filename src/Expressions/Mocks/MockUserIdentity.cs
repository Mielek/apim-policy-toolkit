namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockUserIdentity : IUserIdentity
{
    public string Id { get; set; } = "xPTL3ja8qr";
    public string Provider { get; set; } = "basic";
}