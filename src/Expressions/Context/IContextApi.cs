namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;
public interface IContextApi : IApi
{
    bool IsCurrentRevision { get; }
    string Revision { get; }
    string Version { get; }
}