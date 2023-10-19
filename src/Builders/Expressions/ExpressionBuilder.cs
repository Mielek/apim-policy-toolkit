using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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

    public ExpressionBuilder<T> Inline(Expression<Func<IContext, T>> expression)
    {
        _expression = new InlineExpression<T>(expression);
        return this;
    }

    public ExpressionBuilder<T> Lambda(Func<IContext, T> lambda, [CallerArgumentExpression(nameof(lambda))] string? lambdaCode = null)
    {
        if (lambdaCode == null)
        {
            throw new Exception("Compiler services are not enabled");
        }
        _expression = new LambdaExpression<T>(lambda.GetMethodInfo(), lambdaCode);
        return this;
    }

    public ExpressionBuilder<T> Method(Func<IContext, T> method,  [CallerFilePath] string sourceFilePath = "")
    {
        _expression = new MethodExpression<T>(method.GetMethodInfo(), sourceFilePath);
        return this;
    }

    public IExpression<T> Build()
    {
        return _expression ?? throw new NullReferenceException();
    }
}