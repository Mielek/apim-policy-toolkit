namespace Mielek.Marshalling;

public sealed class MarshallerOptions
{
    public static MarshallerOptions Default => new MarshallerOptions();

    string _scriptBaseDirectory = Environment.CurrentDirectory;

    public string ScriptBaseDirectory => _scriptBaseDirectory;

    MarshallerOptions() { }

    public MarshallerOptions WithFileScriptBaseDirectory(string scriptBaseDirectory)
    {
        this._scriptBaseDirectory = scriptBaseDirectory;
        return this;
    }
}