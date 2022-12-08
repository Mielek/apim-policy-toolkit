using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

public sealed record CheckHeaderPolicy(
    IExpression Name,
    IExpression FailedCheckHttpCode,
    IExpression FailedCheckErrorMessage,
    IExpression IgnoreCase,
    ICollection<IExpression> Values
) : Visitable<CheckHeaderPolicy>, IPolicy;

public sealed record GetAuthorizationContextPolicy(
    IExpression ProviderId,
    IExpression AuthorizationId,
    string ContextVariableName,
    IdentityType? IdentityType = null,
    IExpression? Identity = null,
    bool? IgnoreError = null
) : Visitable<GetAuthorizationContextPolicy>, IPolicy;
public enum IdentityType { Managed, JWT }

public sealed record RateLimitPolicy(
    uint Calls,
    uint RenewalPeriod,
    string? RetryAfterHeaderName = null,
    string? RetryAfterVariableName = null,
    string? RemainingCallsHeaderName = null,
    string? RemainingCallsVariableName = null,
    string? TotalCallsHeaderName = null,
    ICollection<RateLimitApi>? Apis = null
) : Visitable<RateLimitPolicy>, IPolicy;
public sealed record RateLimitApi(
    uint Calls,
    uint RenewalPeriod,
    string? Name,
    string? Id,
    string? RetryAfterHeaderName = null,
    string? RetryAfterVariableName = null,
    string? RemainingCallsHeaderName = null,
    string? RemainingCallsVariableName = null,
    string? TotalCallsHeaderName = null,
    ICollection<RateLimitApiOperation>? Operations = null
) : Visitable<RateLimitApi>;
public sealed record RateLimitApiOperation(
    uint Calls,
    uint RenewalPeriod,
    string? Name,
    string? Id,
    string? RetryAfterHeaderName = null,
    string? RetryAfterVariableName = null,
    string? RemainingCallsHeaderName = null,
    string? RemainingCallsVariableName = null,
    string? TotalCallsHeaderName = null
) : Visitable<RateLimitApiOperation>;

public sealed record RateLimitByKeyPolicy(
    uint Calls,
    uint RenewalPeriod,
    IExpression CounterKey,
    IExpression? IncrementCondition = null,
    uint? IncrementCount = null,
    string? RetryAfterHeaderName = null,
    string? RetryAfterVariableName = null,
    string? RemainingCallsHeaderName = null,
    string? RemainingCallsVariableName = null,
    string? TotalCallsHeaderName = null
) : Visitable<RateLimitByKeyPolicy>, IPolicy;

public sealed record IpFilterPolicy(IpFilterAction Action, ICollection<IIpFilterValue> Values) : Visitable<IpFilterPolicy>, IPolicy;
public enum IpFilterAction
{
    Allow, Forbid
}
public interface IIpFilterValue { };
public sealed record IpFilterAddress(string Ip) : IIpFilterValue;
public sealed record IpFilterAddressRange(string FromIp, string ToIp) : IIpFilterValue;

public sealed record QuotaPolicy(
    uint RenewalPeriod,
    uint? Calls = null,
    uint? Bandwidth = null,
    ICollection<QuotaApi>? Apis = null
) : Visitable<QuotaPolicy>, IPolicy;
public sealed record QuotaApi(uint Calls, string? Name = null, string? Id = null, ICollection<QuotaOperation>? Operations = null);
public sealed record QuotaOperation(uint Calls, string? Name = null, string? Id = null);

public sealed record QuotaByKeyPolicy(
    IExpression CounterKey,
    uint RenewalPeriod,
    uint? Calls = null,
    uint? Bandwidth = null,
    IExpression? IncrementCondition = null,
    DateTime? FirstPeriodStart = null
) : Visitable<QuotaByKeyPolicy>, IPolicy;

public sealed record ValidateAzureAdTokenPolicy(
    ICollection<string> ClientApplicationIds,
    string? HeaderName = null,
    string? QueryParameterName = null,
    string? TokenValue = null,
    string? TenantId = null,
    ushort? FailedValidationHttpCode = null,
    string? FailedValidationErrorMessage = null,
    string? OutputTokenVariableName = null,
    ICollection<string>? BackendApplicationIds = null,
    ICollection<IExpression>? Audiences = null,
    ICollection<ValidateAzureAdTokenClaim>? RequiredClaims = null
) : Visitable<ValidateAzureAdTokenPolicy>, IPolicy;
public sealed record ValidateAzureAdTokenClaim(
    string Name,
    ICollection<string> Values,
    ValidateAzureAdTokenClaimMatch? Match = null,
    string? Separator = null
);
public enum ValidateAzureAdTokenClaimMatch
{
    All, Any
}

public sealed record ValidateJwtPolicy(
    string? HeaderName = null,
    string? QueryParameterName = null,
    string? TokenValue = null,
    ushort? FailedValidationHttpCode = null,
    string? FailedValidationErrorMessage = null,
    bool? RequireExpirationTime = null,
    string? RequireScheme = null,
    bool? RequireSignedTokens = null,
    uint? ClockSkew = null,
    string? OutputTokenVariableName = null,
    ValidateJwtOpenIdConfig? OpenIdConfig = null,
    ICollection<string>? IssuerSigningKeys = null,
    ICollection<string>? DecryptionKeys = null,
    ICollection<IExpression>? Audiences = null,
    ICollection<string>? Issuers = null,
    ICollection<ValidateJwtClaim>? RequiredClaims = null
) : Visitable<ValidateJwtPolicy>, IPolicy;
public sealed record ValidateJwtOpenIdConfig(string Url);
public sealed record ValidateJwtClaim(
    string Name,
    ICollection<string> Values,
    ValidateJwtClaimMatch? Match = null,
    string? Separator = null
);
public enum ValidateJwtClaimMatch
{
    All, Any
}

public sealed record ValidateClientCertificatePolicy(
    ICollection<ValidateClientCertificateIdentity> Identities,
    bool? ValidateRevocation = null,
    bool? ValidateTrust = null,
    bool? ValidateNotBefore = null,
    bool? ValidateNotAfter = null,
    bool? IgnoreError = null
) : Visitable<ValidateClientCertificatePolicy>, IPolicy;
public record ValidateClientCertificateIdentity(
    string? Thumbprint = null,
    string? SerialNumber = null,
    string? CommonName = null,
    string? Subject = null,
    string? DnsName = null,
    string? IssuerSubject = null,
    string? IssuerThumbprint = null,
    string? IssuerCertificateId = null
);