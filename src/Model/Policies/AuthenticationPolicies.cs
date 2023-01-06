using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

public sealed record AuthenticationBasicPolicy(
    string Username,
    string Password
) : Visitable<AuthenticationBasicPolicy>, IPolicy;

public sealed record AuthenticationCertificatePolicy(
    string? Thumbprint = null,
    string? CertificateId = null,
    IExpression<string>? Body = null,
    string? Password = null
) : Visitable<AuthenticationCertificatePolicy>, IPolicy;

public sealed record AuthenticationManagedIdentity(
    string Resource,
    string? ClientId = null,
    string? OutputTokenVariableName = null,
    bool? IgnoreError = null
) : Visitable<AuthenticationManagedIdentity>, IPolicy;
