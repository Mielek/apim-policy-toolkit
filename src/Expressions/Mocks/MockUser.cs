using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockUser : IUser
{
    public string Email => throw new NotImplementedException();

    public string FirstName => throw new NotImplementedException();

    public IEnumerable<IGroup> Groups => throw new NotImplementedException();

    public string Id => throw new NotImplementedException();

    public IEnumerable<IUserIdentity> Identities => throw new NotImplementedException();

    public string LastName => throw new NotImplementedException();

    public string Note => throw new NotImplementedException();

    public DateTime RegistrationDate => throw new NotImplementedException();
}