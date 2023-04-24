using System.Xml;

namespace Mielek.Marshalling;

public sealed class MarshallerOptions
{
    
    public static MarshallerOptions Default => new();

    public string ScriptBaseDirectory { get; private set; } = Environment.CurrentDirectory;
    public bool FormatXml { get; private set; } = false;
    public IDictionary<string, string> MethodLibrary { get; private set; } = new Dictionary<string, string>();

    MarshallerOptions() { }

    public MarshallerOptions WithFileScriptBaseDirectory(string scriptBaseDirectory)
    {
        ScriptBaseDirectory = scriptBaseDirectory;
        return this;
    }

    public MarshallerOptions WithXmlFormatting(bool value)
    {
        FormatXml = value;
        return this;
    }

    public MarshallerOptions WithMethodSourceLibrary(IDictionary<string, string> methodSourceLibrary)
    {
        MethodLibrary = methodSourceLibrary;
        return this;
    }

    public XmlWriterSettings XmlWriterSettings => new() { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, Indent = FormatXml };
}