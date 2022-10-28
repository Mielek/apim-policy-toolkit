namespace Mielek.Model.Expressions;

public sealed record ConstantExpression(string Value) : Visitable<ConstantExpression>, IExpression;
public sealed record FileScriptExpression(string Path) : Visitable<FileScriptExpression>, IExpression;
public sealed record InlineScriptExpression(string Script) : Visitable<InlineScriptExpression>, IExpression;