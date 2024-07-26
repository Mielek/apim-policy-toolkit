using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Emulator;

public class OutboundContext : IOutboundContext
{
    public void RemoveHeader(string name)
    {
        throw new NotImplementedException();
    }

    public void SetBody(string body)
    {
        throw new NotImplementedException();
    }

    public void Base()
    {
        throw new NotImplementedException();
    }

    public IContext Context { get; }
}