namespace Mielek.Test.Marshalling;

public static class TestScripts
{
    public static string OneLine => ScriptPath("one-line");
    public static string GuidTime => ScriptPath("guid-time");
    public static string FilterBody => ScriptPath("filter-body");
    public static string WithDirective => ScriptPath("with-directive");

    public static string ScriptPath(string scriptName)
    {
        return $"../../../scripts/{scriptName}.csx";
    }
}