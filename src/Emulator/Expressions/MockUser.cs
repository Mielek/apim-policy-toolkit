using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockUser : IUser
{
    public string Email { get; set; } = "joe.doe@contoso.com";

    public string FirstName { get; set; } = "Joe";

    public List<MockGroup> MockGroups { get; set; } = new List<MockGroup>() { new MockGroup() };
    public IEnumerable<IGroup> Groups => MockGroups;

    public string Id { get; set; } = "joe-doe-contoso-com";

    public List<MockUserIdentity> MockUserIdentities { get; set; } = new List<MockUserIdentity>() { new MockUserIdentity() };
    public IEnumerable<IUserIdentity> Identities => MockUserIdentities;

    public string LastName { get; set; } = "Doe";

    public string Note { get; set; } = "Joe likes dogs and cats equally";

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow.AddDays(-5);
}