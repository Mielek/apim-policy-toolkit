namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AddToSectionBuilderAttribute : Attribute
{
    public AddToSectionBuilderAttribute(Type _)
    { }
}