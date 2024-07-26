namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

[AttributeUsage(AttributeTargets.Class)]
public class DocumentAttribute(string? name = null) : Attribute
{
    public string? Name { get; } = name;
}