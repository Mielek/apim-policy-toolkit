namespace Mielek.Expressions.Context;
public interface IContextApi : IApi
{
    bool IsCurrentRevision { get; }
    string Revision { get; }
    string Version { get; }
}