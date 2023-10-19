namespace Mielek.Model.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocumentAttribute : Attribute
{
    public string? Name { get; init; }

}