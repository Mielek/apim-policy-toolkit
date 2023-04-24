using Mielek.Expressions.Context;

namespace Mielek.Model.Expressions;

public sealed record ConstantExpression<T>(T Value) : Visitable<ConstantExpression<T>>, IExpression<T>;
public sealed record FileScriptExpression<T>(string Path) : Visitable<FileScriptExpression<T>>, IExpression<T>;
public sealed record InlineScriptExpression<T>(string Script) : Visitable<InlineScriptExpression<T>>, IExpression<T>;
public sealed record FunctionFileScriptExpression<T>(string Path, string Name) : Visitable<FunctionFileScriptExpression<T>>, IExpression<T>;
public sealed record MethodExpression<T>(Func<IContext, T> Fuc) : Visitable<MethodExpression<T>>, IExpression<T>
{
    public string MethodName => Fuc.Method.Name;
    public string? MethodClass => Fuc.Method.DeclaringType?.FullName;
}

[AttributeUsage(AttributeTargets.Method)]
public class MethodExpressionAttribute : Attribute
{
    public MethodExpressionAttribute()
    { }
}


[AttributeUsage(AttributeTargets.Method)]
public class LambdaExpressionAttribute : Attribute
{
    public LambdaExpressionAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}
