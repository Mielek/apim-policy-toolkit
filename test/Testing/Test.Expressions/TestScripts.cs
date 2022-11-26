namespace Mielek.Test.Testing.Expressions;

public static class TestScripts
{
    public static string WithoutDirective => ScriptPath("without-directive");
    public static string WithDirective => ScriptPath("with-directive");
    public static string Functions => ScriptPath("functions");

    public static string ScriptPath(string scriptName)
    {
        return $"../../../scripts/{scriptName}.csx";
    }
}