using System.Text;
using System.Text.RegularExpressions;

using Compiler;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;
using Mielek.Azure.ApiManagement.PolicyToolkit.Serialization;

var config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();
var options = new CompilerOptions(config);
var files = Directory.GetFiles(options.SourceFolder, "*.cs", SearchOption.AllDirectories).Where(p => !Regex.IsMatch(p, @".*[\\/](obj|bin)[\\/].*"));

foreach (var file in files)
{
    var fileName = Path.GetFileNameWithoutExtension(file);
    var code = File.ReadAllText(file);
    var syntax = CSharpSyntaxTree.ParseText(code);

    var documents = syntax.GetRoot()
        .DescendantNodes()
        .OfType<ClassDeclarationSyntax>()
        .Where(c => c.AttributeLists.ContainsAttributeOfType("Document"));
    foreach (var document in documents)
    {
        var result = new CSharpPolicyCompiler(document).Compile();

        foreach (var error in result.Errors)
        {
            Console.Out.WriteLine(error);
        }

        var codeBuilder = new StringBuilder();
        using (var writer = CustomXmlWriter.Create(codeBuilder, options.XmlWriterSettings))
        {
            writer.Write(result.Document);
        }

        var xml = codeBuilder.ToString();
        if (options.Format)
        {
            xml = RazorCodeFormatter.Format(xml);
        }

        var policyFileName = document.ExtractDocumentFileName();
        policyFileName = policyFileName.EndsWith(".xml") ? policyFileName : $"{policyFileName}.xml";

        string targetFolder = Path.GetFullPath(Path.Combine(options.OutputFolder, Path.GetFullPath(file).Split(Path.GetFullPath(options.SourceFolder))[1].Replace(Path.GetFileName(file), "")));
        var targetFile = Path.Combine(targetFolder, policyFileName);
        var directoryPath = Path.GetDirectoryName(targetFile);
        
        if (directoryPath is not null && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        File.WriteAllText(targetFile, xml);
        Console.Out.WriteLine($"File {targetFile} created");
    }

    Console.Out.WriteLine($"File {fileName} processed");
}