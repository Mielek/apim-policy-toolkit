namespace Mielek.Expressions.Context;
public interface IUrl
{
    string Host { get; }
    string Path { get; }
    string Port { get; }
    IReadOnlyDictionary<string, string[]> Query { get; }
    string QueryString { get; }
    string Scheme { get; }
}