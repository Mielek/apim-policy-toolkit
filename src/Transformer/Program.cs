using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Configuration;

using Mielek.Builders;
using Mielek.Marshalling;
using Mielek.Model;

var options = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var sourceFolder = options["s"] ?? options["source"] ?? throw new NullReferenceException("Source folder not provided");
var targetFolder = options["t"] ?? options["target"] ?? throw new NullReferenceException("Target folder not provided");

var scriptExtensionPattern = options["e"] ?? options["extension"] ?? "*.csx";

var addRefsAndImportsToScripts = bool.Parse(options["add-imports"] ?? "true");
var removeDirectivesFromScripts = bool.Parse(options["remove-directives"] ?? "false");
var directivesRegex = new Regex("^#.* .*$", RegexOptions.Multiline);

var scriptOptions = ScriptOptions.Default
        .AddReferences(typeof(IVisitable).Assembly, typeof(PolicyDocumentBuilder).Assembly);

if (addRefsAndImportsToScripts)
{
    var imports = new[] {
        "Mielek.Builders",
        "Mielek.Builders.Policies",
        "Mielek.Builders.Expressions",
        "Mielek.Model",
        "Mielek.Model.Policies",
        "Mielek.Model.Expressions"
    };
    scriptOptions = scriptOptions.AddImports(imports);
}

var marshallerOptions = MarshallerOptions.Default.WithFileScriptBaseDirectory(options["script-source"] ?? sourceFolder);

var files = Directory.GetFiles(sourceFolder, scriptExtensionPattern);

Task.WaitAll(files.Select(async filePath =>
{
    var fileName = Path.GetFileNameWithoutExtension(filePath);
    var code = File.ReadAllText(filePath);

    if (removeDirectivesFromScripts)
    {
        code = directivesRegex.Replace(code, "");
    }

    var policy = await CSharpScript.EvaluateAsync<IVisitable>(code, scriptOptions);

    var targetFile = Path.Combine(targetFolder, $"{fileName}.xml");

    if (File.Exists(targetFile))
    {
        File.Delete(targetFile);
    }

    using (var marshaller = Marshaller.Create(targetFile, marshallerOptions))
    {
        policy.Accept(marshaller);
    }

    Console.Out.WriteLine($"Document {fileName} created");
}).ToArray());