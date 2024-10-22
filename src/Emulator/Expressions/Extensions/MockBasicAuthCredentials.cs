using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public record MockBasicAuthCredentials(string Username, string Password) : BasicAuthCredentials
{
}