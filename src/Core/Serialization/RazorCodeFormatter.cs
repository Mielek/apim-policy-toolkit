using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Serialization;
public class RazorCodeFormatter
{
    private static readonly Regex CSharpCodeStart = new Regex("(@\\()|(@{)", RegexOptions.Compiled);
    private readonly string code;
    private readonly StringBuilder result = new StringBuilder();

    public RazorCodeFormatter(string code)
    {
        this.code = code;
    }

    public string Format()
    {
        var lastIndex = 0;
        foreach (Match match in CSharpCodeStart.Matches(code))
        {
            result.Append(code, lastIndex, match.Index - lastIndex + 2);
            var index = FindClosingIndex(match, out bool isMultiline);
            if (isMultiline) result.AppendLine();

            var cSharpCode = code.Substring(match.Index + 2, index - match.Index - 2);
            var formattedCode = FormatCSharpCode(cSharpCode);
            result.Append(formattedCode);

            if (isMultiline) result.AppendLine();

            lastIndex = index;
        }

        result.Append(code, lastIndex, code.Length - lastIndex);
        return result.ToString();
    }

    private int FindClosingIndex(Match match, out bool isMultiline)
    {
        var index = match.Index + 2;
        int open = 1;
        var openCharacter = code[match.Index + 1];
        isMultiline = openCharacter == '{';
        var closeCharacter = isMultiline ? '}' : ')';
        do
        {
            var character = code[index++];
            if (character == openCharacter) ++open;
            else if (character == closeCharacter) --open;
        } while (open > 0);

        return --index;
    }

    private string FormatCSharpCode(string code)
    {
        return CSharpSyntaxTree.ParseText(code)
            .GetRoot()
            .NormalizeWhitespace()
            .ToFullString()
            .Trim();
    }
}