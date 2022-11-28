using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

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