using System.Diagnostics;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;

using Mielek.Marshalling;
using Mielek.Model;
using Mielek.Model.Attributes;

var options = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var dllFile = options["dllFile"] ?? throw new Exception();
var output = options["out"] ?? throw new Exception();
var format = bool.TryParse(options["format"] ?? "false", out var fmt) && fmt;
var formatXML = bool.TryParse(options["format-xml"] ?? "false", out var fmtXML) && fmtXML;
var formatCSharp = bool.TryParse(options["format-csharp"] ?? "false", out var fmtCSharp) && fmtCSharp;

var marshallerOptions = MarshallerOptions.Default
    .WithXmlFormatting(format || formatXML)
    .WithCSharpFormatting(format || formatCSharp);

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
        if (document.ReturnType != typeof(PolicyDocument))
        {
            // error message
            continue;
        }

        if (document.GetParameters().Length != 0)
        {
            // error message
            continue;
        }
        Console.Out.WriteLine($"Document of {document}");

        var policyDoc = document.Invoke(instance, null) as PolicyDocument;
        var targetFile = Path.Combine(output, $"{type.Name}.{document.Name}.xml");
        Console.Out.WriteLine($"Create {targetFile}");
        using (var marshaller = Marshaller.Create(targetFile, marshallerOptions))
        {
            policyDoc?.Accept(marshaller);
        }
    }
}

Console.Out.WriteLine($"Generation finished in {stopwatch.Elapsed} time");
