using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockContextApi : MockApi, IContextApi
{
    public bool IsCurrentRevision => throw new NotImplementedException();

    public string Revision => throw new NotImplementedException();

    public string Version => throw new NotImplementedException();
}