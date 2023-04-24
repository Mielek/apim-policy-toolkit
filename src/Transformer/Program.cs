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

var sourceFolder = options["s"] ?? options["source"] ?? ".";
var targetFolder = options["t"] ?? options["target"] ?? "./target";

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
        "Mielek.Model.Expressions",
        "Mielek.Expressions.Context"
    };
    scriptOptions = scriptOptions.AddImports(imports);
}

if (!Directory.Exists(targetFolder))
{
    Console.Out.WriteLine($"Target folder does not exist. Creating {targetFolder} structure");
    Directory.CreateDirectory(targetFolder);
}

var marshallerOptions = MarshallerOptions.Default
    .WithFileScriptBaseDirectory(options["script-source"] ?? sourceFolder)
    .WithXmlFormatting(bool.Parse(options["format-output"] ?? "true"));

var files = Directory.GetFiles(sourceFolder, scriptExtensionPattern);

Task.WaitAll(files.Select(async filePath =>
{
    var fileName = Path.GetFileNameWithoutExtension(filePath);
    var code = File.ReadAllText(filePath);

    if (removeDirectivesFromScripts)
    {
        code = directivesRegex.Replace(code, "");
    }

    var lib = new FileMethodLibraryResolver(code).ResolveMethodLibrary();

    var policy = await CSharpScript.RunAsync<IVisitable>(code, scriptOptions);

    var targetFile = Path.Combine(targetFolder, $"{fileName}.xml");

    if (File.Exists(targetFile))
    {
        Console.Out.WriteLine($"Previous file detected. Removing {fileName}.xml");
        File.Delete(targetFile);
    }

    using (var marshaller = Marshaller.Create(targetFile, marshallerOptions.WithMethodSourceLibrary(lib)))
    {
        policy.ReturnValue.Accept(marshaller);
    }

    Console.Out.WriteLine($"Document {fileName}.xml created");
}).ToArray());