namespace Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class LibraryAttribute : Attribute
{
    public string? Name { get; init; }
}