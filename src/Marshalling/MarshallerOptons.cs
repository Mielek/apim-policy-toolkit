using System.Xml;

namespace Mielek.Marshalling;

public sealed class MarshallerOptions
{
    
    public static MarshallerOptions Default => new();

    public bool FormatXml { get; private set; } = false;
    public bool FormatCSharp { get; private set; } = false;

    MarshallerOptions() { }

    public MarshallerOptions WithXmlFormatting(bool value)
    {
        FormatXml = value;
        return this;
    }

    public MarshallerOptions WithCSharpFormatting(bool value)
    {
        FormatCSharp = value;
        return this;
    }

    public XmlWriterSettings XmlWriterSettings => new() { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, Indent = FormatXml };
}