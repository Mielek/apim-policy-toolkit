namespace Mielek.Model.Expressions;

public sealed record ConstantExpression<T>(T Value) : Visitable<ConstantExpression<T>>, IExpression<T>;
public sealed record FileScriptExpression<T>(string Path) : Visitable<FileScriptExpression<T>>, IExpression<T>;
public sealed record InlineScriptExpression<T>(string Script) : Visitable<InlineScriptExpression<T>>, IExpression<T>;
public sealed record FunctionFileScriptExpression<T>(string Path, string Name) : Visitable<FunctionFileScriptExpression<T>>, IExpression<T>;