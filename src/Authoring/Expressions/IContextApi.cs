namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public interface IContextApi : IApi
{
    bool IsCurrentRevision { get; }
    string Revision { get; }
    string Version { get; }
}