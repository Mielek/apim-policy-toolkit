namespace Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CodeDocumentAttribute : Attribute
{
    public string? Name { get; }

    public CodeDocumentAttribute(string? name = null)
    {
        Name = name;
    }
}