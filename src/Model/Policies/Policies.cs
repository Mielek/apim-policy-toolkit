using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

public record BasePolicy() : Visitable<BasePolicy>, IPolicy;

public record SetStatusPolicy(IExpression Code, IExpression? Reason = null) : Visitable<SetStatusPolicy>, IPolicy;

public record SetMethodPolicy(IExpression Method) : Visitable<SetMethodPolicy>, IPolicy;

public record SetHeaderPolicy(IExpression Name, ICollection<IExpression>? Values = null, IExpression? ExistAction = null) : Visitable<SetHeaderPolicy>, IPolicy;
public enum ExistAction { Override, Skip, Append, Delete }

public record SetBodyPolicy(IExpression Body, IExpression? Template = null, IExpression? XsiNil = null) : Visitable<SetBodyPolicy>, IPolicy;
public enum BodyTemplate { Liquid }
public enum XsiNilType { Blank, Null }

public record CheckHeaderPolicy(IExpression Name, IExpression FailedCheckCode, IExpression FailedCheckErrorMessage, IExpression IgnoreCase, ICollection<IExpression> Values) : Visitable<CheckHeaderPolicy>, IPolicy;
