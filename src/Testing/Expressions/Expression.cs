using Mielek.Expressions.Context;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Mielek.Testing.Expressions;

public sealed class Expression
{
    static readonly Assembly[] Assemblies = new[] {
        typeof(object).Assembly,
        typeof(GlobalType).Assembly,
        typeof(IContext).Assembly,
        typeof(JsonToken).Assembly,
        typeof(JObject).Assembly
    };

    static readonly string[] Imports = new[] {
        "System", "Mielek.Expressions.Context", "Newtonsoft.Json", "Newtonsoft.Json.Linq"
    };

    static readonly ScriptOptions CSharpScriptOptions = ScriptOptions.Default
        .AddReferences(Assemblies)
        .AddImports(Imports);

    readonly Script<string> _script;

    public Expression(string code)
    {
        _script = CSharpScript.Create<string>(code, CSharpScriptOptions, typeof(GlobalType));
        _script.Compile();
    }

    public async Task<string> Execute(IContext context, CancellationToken token = default)
    {
        var state = await _script.RunAsync(new GlobalType(context), token);
        return state.ReturnValue;
    }
}
