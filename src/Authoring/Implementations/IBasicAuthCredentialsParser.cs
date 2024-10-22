using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Implementations;

public interface IBasicAuthCredentialsParser
{
    BasicAuthCredentials? Parse(string? value);
}