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
foreach (var libraryType in libraries)
{
    var libraryName = libraryType.GetCustomAttribute<LibraryAttribute>()?.Name ?? libraryType.Name;
    var instance = Activator.CreateInstance(libraryType);
    var documents = libraryType.GetMethods().Where(m => m.GetCustomAttribute<DocumentAttribute>() != null);

    foreach (var documentMethod in documents)
    {
        if (documentMethod.ReturnType != typeof(XElement) || documentMethod.GetParameters().Length != 0)
        {
            Console.Out.WriteLine($"Method {documentMethod.Name} should be accept no parameters and return XElement type");
            continue;
        }

        var documentName = documentMethod.GetCustomAttribute<DocumentAttribute>()?.Name ?? documentMethod.Name;
        
        var fullName =  $"{libraryName}.{documentName}";
        Console.Out.WriteLine($"Document of {fullName}");
        var policyDocumentValue = documentMethod.Invoke(instance, null) as XElement;

        if (policyDocumentValue == null)
        {
            Console.Out.WriteLine($"Method {documentMethod.Name} returned {policyDocumentValue}");
            continue;
        }

        var codeBuilder = new StringBuilder();
        using (var writer = CustomXmlWriter.Create(codeBuilder, writerSettings))
        {
            writer.Write(policyDocumentValue);
        }
        var code = codeBuilder.ToString();

        if (formatCSharp)
        {
            code = new RazorCodeFormatter(code).Format();
        }

        var targetFile = Path.Combine(output, $"{fullName}.xml");
        File.WriteAllText(targetFile, code);

        Console.Out.WriteLine($"Created {targetFile}");
    }
}

Console.Out.WriteLine($"Generation finished in {stopwatch.Elapsed} time");
