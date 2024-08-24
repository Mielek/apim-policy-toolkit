using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockUserIdentity : IUserIdentity
{
    public string Id { get; set; } = "xPTL3ja8qr";
    public string Provider { get; set; } = "basic";
}