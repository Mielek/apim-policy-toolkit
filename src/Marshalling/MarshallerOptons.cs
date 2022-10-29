namespace Mielek.Marshalling;

public sealed class MarshallerOptions
{
    public static MarshallerOptions Default => new();

    public string ScriptBaseDirectory { get; private set; } = Environment.CurrentDirectory;

    MarshallerOptions() { }

    public MarshallerOptions WithFileScriptBaseDirectory(string scriptBaseDirectory)
    {
        ScriptBaseDirectory = scriptBaseDirectory;
        return this;
    }
}