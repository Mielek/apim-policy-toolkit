using System.Text.RegularExpressions;

namespace Mielek.Expressions.Testing;
public static class ExpressionProvider
{
    readonly static Regex DirectivesRegex = new Regex("^#.* .*$", RegexOptions.Multiline);
    
    public static Expression LoadFromFile(string path)
    {
        var code = File.ReadAllText(path);
        code = DirectivesRegex.Replace(code, "").Trim();
        return new Expression(code);
    }

    public static Expression LoadInline(string code)
    {
        return new Expression(code);
    }
}
