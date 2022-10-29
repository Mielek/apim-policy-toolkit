using System.Text.RegularExpressions;
using System.Xml;

using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class FileScriptExpressionHandler : MarshallerHandler<FileScriptExpression>
{
    readonly static Regex ReferenceRegex = new Regex("^#.* .*$", RegexOptions.Multiline);
    public override void Marshal(Marshaller marshaller, FileScriptExpression element)
    {
        var script = File.ReadAllText(Path.Combine(marshaller.Options.ScriptBaseDirectory, element.Path));
        script = ReferenceRegex.Replace(script, "").Trim();
        marshaller.Writer.WriteString($"@{{{Environment.NewLine}{script}{Environment.NewLine}}}");
    }
}