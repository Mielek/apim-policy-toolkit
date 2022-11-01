using Mielek.Expressions.Context;

namespace Mielek.Testing.Expressions.Mocks;

public class MockContextApi : MockApi, IContextApi
{
    public bool IsCurrentRevision => throw new NotImplementedException();

    public string Revision => throw new NotImplementedException();

    public string Version => throw new NotImplementedException();
}