using Mielek.Expressions.Context;
using Mielek.Model.Expressions;

namespace Mielek.Builders.Expressions;

public class ExpressionBuilder<T>
{
    public static ExpressionBuilder<T> Builder => new();

    public static IExpression<T> BuildFromConfiguration(Action<ExpressionBuilder<T>> configurator)
    {
        var builder = ExpressionBuilder<T>.Builder;
        configurator(builder);
        return builder.Build();
    }

    IExpression<T>? _expression;

    internal ExpressionBuilder() { }

    public ExpressionBuilder<T> Constant(T value)
    {
        _expression = new ConstantExpression<T>(value);
        return this;
    }
    public ExpressionBuilder<T> Inlined(System.Linq.Expressions.Expression<Func<IContext, T>> script)
    {
        _expression = new InlineScriptExpression<T>(script.Body.ToString());
        return this;
    }
    public ExpressionBuilder<T> FromFile(string filePath)
    {
        _expression = new FileScriptExpression<T>(filePath);
        return this;
    }

    public ExpressionBuilder<T> FromFunctionFile(string filePath, string name)
    {
        _expression = new FunctionFileScriptExpression<T>(filePath, name);
        return this;
    }

    public IExpression<T> Build()
    {
        return _expression ?? throw new NullReferenceException();
    }
}