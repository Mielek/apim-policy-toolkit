using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

public sealed record SetHeaderPolicy(
    IExpression<string> Name,
    ICollection<IExpression<string>>? Values = null,
    IExpression<string>? ExistAction = null
) : Visitable<SetHeaderPolicy>, IPolicy;
public enum ExistAction { Override, Skip, Append, Delete }

public sealed record SetBodyPolicy(
    IExpression<string> Body,
    IExpression<string>? Template = null,
    IExpression<string>? XsiNil = null
) : Visitable<SetBodyPolicy>, IPolicy;
public enum BodyTemplate { Liquid }
public enum XsiNilType { Blank, Null }