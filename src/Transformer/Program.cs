using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;

using Mielek.Transformer;
using Mielek.Model.Attributes;

var options = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var dllFile = options["dllFile"] ?? throw new Exception();
var output = options["out"] ?? throw new Exception();
var format = bool.TryParse(options["format"] ?? "false", out var fmt) && fmt;
var formatXml = bool.TryParse(options["format-xml"] ?? "false", out var fmtXML) && fmtXML;
var formatCSharp = bool.TryParse(options["format-csharp"] ?? "false", out var fmtCSharp) && fmtCSharp;

var writerSettings = new XmlWriterSettings()
{
    OmitXmlDeclaration = true,
    ConformanceLevel = ConformanceLevel.Fragment,
    Indent = format || formatXml
};

var stopwatch = Stopwatch.StartNew();
var assembly = Assembly.LoadFrom(dllFile);
Console.Out.WriteLine($"Loaded assembly in {stopwatch.Elapsed}");

var libraries = assembly.GetExportedTypes().Where(t => t.GetCustomAttribute<LibraryAttribute>() != null);
foreach (var type in libraries)
{
    var instance = Activator.CreateInstance(type);
    Console.Out.WriteLine($"Created instance of {type}");

    var documents = type.GetMethods().Where(m => m.GetCustomAttribute<DocumentAttribute>() != null);

    foreach (var document in documents)
    {
        if (document.ReturnType != typeof(XElement) || document.GetParameters().Length != 0)
        {
            Console.Out.WriteLine($"Method {document.Name} should be accept no parameters and return PolicyDocument type");
            continue;
        }

        Console.Out.WriteLine($"Document of {document}");
        var policyDoc = document.Invoke(instance, null) as XElement;

        if (policyDoc == null)
        {
            Console.Out.WriteLine($"Method {document.Name} returned {policyDoc}");
            continue;
        }

        var targetFile = Path.Combine(output, $"{type.Name}.{document.Name}.xml");
        using (var writer = CustomXmlWriter.Create(targetFile, writerSettings))
        {
            writer.Write(policyDoc);
        }

        Console.Out.WriteLine($"Created {targetFile}");
    }
}

Console.Out.WriteLine($"Generation finished in {stopwatch.Elapsed} time");
