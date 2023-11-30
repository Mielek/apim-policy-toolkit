namespace Mielek.Model.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DocumentAttribute : Attribute
{
    public string? Name { get; init; }

}