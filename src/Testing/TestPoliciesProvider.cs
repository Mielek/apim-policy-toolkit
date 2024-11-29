using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

namespace Azure.ApiManagement.PolicyToolkit.Testing;

public class TestPoliciesProvider<TContext> where TContext : class
{
    internal readonly SectionContextProxy SectionContextProxy;
    internal TContext SectionContext => SectionContextProxy as TContext ?? throw new InvalidOperationException();

    internal TestPoliciesProvider(GatewayContext gatewayContext)
    {
        this.SectionContextProxy = gatewayContext.GetSectionProxy<TContext>();
    }
}