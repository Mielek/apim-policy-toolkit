namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Common;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
        => string.Join("", value.Split('-').Select(p => p.Capitalize()));

    public static string Capitalize(this string value)
        => $"{char.ToUpperInvariant(value[0])}{value[1..]}";
    
    public static string VariableName(this string value)
        => $"{char.ToLowerInvariant(value[0])}{value[1..]}";

    public static string Intend(this string value, int indent)
        => value.PadLeft(value.Length + 4 * indent, ' ');

    public static string ToPolicyClassName(this string value)
        => value.ToCamelCase() + "Policy";
}