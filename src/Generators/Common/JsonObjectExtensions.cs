using System.Text.Json.Nodes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Common;

public static class JsonObjectExtensions
{

    public static string Type(this JsonObject value)
        => value["type"]?.GetValue<string>() ?? throw new NullReferenceException(value.GetPathTo("type"));

    public static string? Name(this JsonObject value)
        => value["name"]?.GetValue<string>();
    public static string GetPathToName(this JsonObject value)
        => value.GetPathTo("name");

    public static string? Target(this JsonObject value)
        => value["target"]?.GetValue<string>();
    public static string GetPathToTarget(this JsonObject value)
        => value.GetPathTo("target");

    public static bool Optional(this JsonObject value)
        => value.GetPropertyBool("optional");

    public static bool Expression(this JsonObject value)
        => value.GetPropertyBool("expression");

    private static bool GetPropertyBool(this JsonObject value, string property)
        => value[property]?.GetValue<bool>() ?? false;

    public static JsonObject Properties(this JsonObject value)
        => value.GetPropertyObject("properties");

    public static JsonObject Items(this JsonObject value)
        => value.GetPropertyObject("items");

    private static JsonObject GetPropertyObject(this JsonObject value, string property)
        => value[property]?.AsObject() ?? throw new NullReferenceException($"{value.GetPath()}.{property}");

    public static string[]? Enum(this JsonObject value)
    {
        var enumValues = value["enum"]?.AsArray();
        return enumValues?.Select((n, i) => n?.GetValue<string>() ?? throw new NullReferenceException($"{value.GetPath()}.enum[{i}]")).ToArray();
    }
    public static string GetPathToEnum(this JsonObject value)
        => value.GetPathTo("enum");

    public static string GetPathTo(this JsonObject value, string property)
        => $"{value.GetPath()}.{property}";
}