namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

public static class Policy
{
    public static PolicyDocumentBuilder Document() => new PolicyDocumentBuilder();
    public static PolicyFragmentBuilder Fragment() => new PolicyFragmentBuilder();
}