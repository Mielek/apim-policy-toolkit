namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class GenerateBuilderAttribute : Attribute
{
    public GenerateBuilderAttribute(Type _)
    { }
}
