namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder.Extensions;

public static class StringExtensions
{
    public static string ToMethodName(this string variableName)
    {
        var methodName = variableName.Replace("_", "");
        methodName = $"{methodName[0].ToString().ToUpper()}{methodName.Remove(0, 1)}";
        return methodName;
    }

    public static string ToListMethodName(this string variableName)
    {
        var methodName = variableName.ToMethodName();
        return methodName.Remove(methodName.Length - 1); // remove trailing 's'
    }
}