// using System.Text.Json.Nodes;

// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.Text;

// using Mielek.Generators.Common;

// namespace Mielek.Generators.Builder;

// public class JsonToBuilderCompiler
// {

//     private readonly GeneratorExecutionContext context;

//     public JsonToBuilderCompiler(GeneratorExecutionContext context)
//     {
//         this.context = context;
//     }

//     public string Compile(string fileName, SourceText text)
//     {
//         try
//         {
//             var root = JsonNode.Parse(text.ToString())?.AsObject() ?? throw new NullReferenceException("root is null");
//             return new InnerCompiler(context, fileName.ToCamelCase() + "Policy", root, true).Compile();
//         }
//         catch (Exception e)
//         {
//             context.ReportDiagnostic(Diagnostic.Create(anyError, null, e.Message));
//             return "";
//         }
//     }
// }

// internal class InnerCompiler
// {
//     private GeneratorExecutionContext _context;
//     private object _value;
//     private JsonObject _root;
//     private bool _v;

//     public InnerCompiler(GeneratorExecutionContext context, object value, JsonObject root, bool v)
//     {
//         _context = context;
//         _value = value;
//         _root = root;
//         _v = v;
//     }
// }