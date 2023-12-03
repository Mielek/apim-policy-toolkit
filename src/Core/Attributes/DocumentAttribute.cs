namespace Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DocumentAttribute : Attribute
{
    public string? Name { get; init; }

}