using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;

using Mielek.Azure.ApiManagement.PolicyToolkit.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Serialization;

var options = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var dllFile = options["dllFile"] ?? throw new Exception();
var output = options["out"] ?? throw new Exception();
var format = bool.TryParse(options["format"] ?? "false", out var fmt) && fmt;
var formatXml = bool.TryParse(options["format-xml"] ?? "false", out var fmtXML) && fmtXML || format;
var formatCSharp = bool.TryParse(options["format-csharp"] ?? "false", out var fmtCSharp) && fmtCSharp || format;

var writerSettings = new XmlWriterSettings()
{
    OmitXmlDeclaration = true,
    ConformanceLevel = ConformanceLevel.Fragment,
    Indent = formatXml
};

var stopwatch = Stopwatch.StartNew();
var assembly = Assembly.LoadFrom(dllFile);
Console.Out.WriteLine($"Loaded assembly in {stopwatch.Elapsed}");

var libraries = assembly.GetExportedTypes().Where(t => t.GetCustomAttribute<LibraryAttribute>() != null);
foreach (var type in libraries)
{
    var instance = Activator.CreateInstance(type);
    var documents = type.GetMethods().Where(m => m.GetCustomAttribute<DocumentAttribute>() != null);

    foreach (var document in documents)
    {
        if (document.ReturnType != typeof(XElement) || document.GetParameters().Length != 0)
        {
            Console.Out.WriteLine($"Method {document.Name} should be accept no parameters and return PolicyDocument type");
            continue;
        }

        var documentName = $"{type.Name}.{document.Name}";
        Console.Out.WriteLine($"Document of {documentName}");
        var policyDoc = document.Invoke(instance, null) as XElement;

        if (policyDoc == null)
        {
            Console.Out.WriteLine($"Method {document.Name} returned {policyDoc}");
            continue;
        }

        StringBuilder codeBuilder = new StringBuilder();
        using (var writer = CustomXmlWriter.Create(codeBuilder, writerSettings))
        {
            writer.Write(policyDoc);
        }
        var code = codeBuilder.ToString();

        if (formatCSharp)
        {
            code = new RazorCodeFormatter(code).Format();
        }

        var targetFile = Path.Combine(output, $"{documentName}.xml");
        File.WriteAllText(targetFile, code);

        Console.Out.WriteLine($"Created {targetFile}");
    }
}

Console.Out.WriteLine($"Generation finished in {stopwatch.Elapsed} time");
