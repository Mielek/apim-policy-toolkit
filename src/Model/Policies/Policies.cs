using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

public sealed record BasePolicy() : Visitable<BasePolicy>, IPolicy;

public sealed record SetStatusPolicy(IExpression Code, IExpression? Reason = null) : Visitable<SetStatusPolicy>, IPolicy;

public sealed record SetMethodPolicy(IExpression Method) : Visitable<SetMethodPolicy>, IPolicy;

public sealed record SetHeaderPolicy(
    IExpression Name,
    ICollection<IExpression>? Values = null,
    IExpression? ExistAction = null
) : Visitable<SetHeaderPolicy>, IPolicy;
public enum ExistAction { Override, Skip, Append, Delete }

public sealed record SetBodyPolicy(
    IExpression Body,
    IExpression? Template = null,
    IExpression? XsiNil = null
) : Visitable<SetBodyPolicy>, IPolicy;
public enum BodyTemplate { Liquid }
public enum XsiNilType { Blank, Null }

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
    string Name,
    uint Calls,
    uint RenewalPeriod,
    string? RetryAfterHeaderName = null,
    string? RetryAfterVariableName = null,
    string? RemainingCallsHeaderName = null,
    string? RemainingCallsVariableName = null,
    string? TotalCallsHeaderName = null,
    ICollection<RateLimitApiOperation>? Operations = null
) : Visitable<RateLimitApi>;
public sealed record RateLimitApiOperation(
    string Name,
    uint Calls,
    uint RenewalPeriod,
    string? RetryAfterHeaderName = null,
    string? RetryAfterVariableName = null,
    string? RemainingCallsHeaderName = null,
    string? RemainingCallsVariableName = null,
    string? TotalCallsHeaderName = null
) : Visitable<RateLimitApiOperation>;


public sealed record IncludeFragmentPolicy(string FragmentId) : Visitable<IncludeFragmentPolicy>, IPolicy;