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

public sealed record IncludeFragmentPolicy(string FragmentId) : Visitable<IncludeFragmentPolicy>, IPolicy;