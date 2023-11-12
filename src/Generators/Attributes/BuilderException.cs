using System.Text;

namespace Mielek.Generators.Attributes;
public class BuilderException : Exception
{
    public BuilderException(Dictionary<string, string> errors) : this(null, errors)
    {
    }

    public BuilderException(string message, Dictionary<string, string> errors) : base(GetErrorMessage(message, errors))
    {
        Errors = errors;
    }

    private static string GetErrorMessage(string message, Dictionary<string, string> errors)
    {
        if (message != null)
            return message;
        var sb = new StringBuilder();
        sb.AppendLine("Error building object. The following properties have errors:");
        foreach (var error in errors)
        {
            sb.AppendLine($"   {error.Key}: {error.Value}");
        }

        return sb.ToString();
    }

    public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();
}