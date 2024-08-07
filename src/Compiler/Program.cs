using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;
using Mielek.Azure.ApiManagement.PolicyToolkit.Serialization;

var options = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var format = bool.TryParse(options["format"] ?? "false", out var fmt) && fmt;
var sourceFolder = options["s"] ?? options["source"] ?? throw new NullReferenceException("Source folder not provided");
var output = options["o"] ?? options["out"] ?? throw new NullReferenceException("Output folder not provided");

var scriptExtensionPattern = options["e"] ?? options["extension"] ?? "*.cs";

var writerSettings = new XmlWriterSettings()
{
    OmitXmlDeclaration = true,
    ConformanceLevel = ConformanceLevel.Fragment,
    Indent = format
};

var files = Directory.GetFiles(sourceFolder, scriptExtensionPattern);

foreach (var file in files)
{
    var fileName = Path.GetFileNameWithoutExtension(file);
    var code = File.ReadAllText(file);
    var syntax = CSharpSyntaxTree.ParseText(code);

    var Documents = syntax.GetRoot()
        .DescendantNodes()
        .OfType<ClassDeclarationSyntax>()
        .Where(c => c.AttributeLists.ContainsAttributeOfType("Document"));
    foreach (var document in Documents)
    {
        var result = new CSharpPolicyCompiler(document).Compile();

        foreach(var error in result.Errors)
        {
            Console.Out.WriteLine(error);
        }

        var codeBuilder = new StringBuilder();
        using (var writer = CustomXmlWriter.Create(codeBuilder, writerSettings))
        {
            writer.Write(result.Document);
        }

        var xml = codeBuilder.ToString();
        if (format)
        {
            xml = new RazorCodeFormatter(xml).Format();
        }

        var attributeSyntax = document.AttributeLists.GetFirstAttributeOfType("Document");
        var policyFileName =
            (attributeSyntax?.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?.Token
            .ValueText ?? document.Identifier.ValueText;

        var targetFile = Path.Combine(output, $"{policyFileName}.xml");
        File.WriteAllText(targetFile, xml);
        Console.Out.WriteLine($"File {targetFile} created");
    }

    Console.Out.WriteLine($"File {fileName} processed");
}