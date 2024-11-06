// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml;

using Microsoft.Extensions.Configuration;

namespace Compiler;

public class CompilerOptions
{
    public string SourceFolder { get; }
    public string OutputFolder { get; }
    public bool Format { get; }

    public XmlWriterSettings XmlWriterSettings => new XmlWriterSettings()
    {
        OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, Indent = Format
    };

    public CompilerOptions(IConfigurationRoot configuration)
    {
        SourceFolder = configuration["s"] ??
                       configuration["source"] ??
                       throw new NullReferenceException("Source folder not provided");
        OutputFolder = configuration["o"] ??
                       configuration["out"] ??
                       throw new NullReferenceException("Output folder not provided");

        Format = bool.TryParse(configuration["format"] ?? "false", out var fmt) && fmt;
    }
}