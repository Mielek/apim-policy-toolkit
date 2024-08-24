// using System.Text;

// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.Text;

// using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Common;

// namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder;

// [Generator]
// public class BuilderGenerator : ISourceGenerator
// {
//     public readonly static DiagnosticDescriptor CouldNotReadFile = new DiagnosticDescriptor(
//             "GEN001",
//             "Cannot get file text",
//             "File '{0}' could not be read in generation",
//             "Generator",
//             DiagnosticSeverity.Warning,
//             isEnabledByDefault: true);

//     public void Initialize(GeneratorInitializationContext context) { }

//     public void Execute(GeneratorExecutionContext context)
//     {
//         var compiler = new JsonToBuilderCompiler(context);
//         var schemaFiles = context.AdditionalFiles.Where(at => at.Path.EndsWith(".json"));

//         foreach (var file in schemaFiles)
//         {
//             var content = file.GetText(context.CancellationToken);
//             if (content != null)
//             {
//                 var fileName = Path.GetFileNameWithoutExtension(file.Path);
//                 var output = compiler.Compile(Path.GetFileNameWithoutExtension(fileName), content);
//                 var generated = $$"""
//                 using Mielek.Model.Expressions;

//                 namespace Mielek.Model.Policies;

//                 {{output}}
//                 """;
//                 var sourceText = SourceText.From(generated, Encoding.UTF8);
//                 context.AddSource($"{fileName}.model.g.cs", sourceText);
//             }
//             else
//             {
//                 context.ReportDiagnostic(Diagnostic.Create(CouldNotReadFile, null, file.Path));
//             }
//         }
//     }
// }