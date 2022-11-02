using Mielek.Model;
using Mielek.Model.Expressions;

namespace Mielek.Builders.Expressions;

public class ExpressionBuilder
{
    public static ExpressionBuilder Builder => new();

    public static IExpression BuildFromConfiguration(Action<ExpressionBuilder> configurator)
    {

        var builder = Builder;
        configurator(builder);
        return builder.Build();
    }

    IExpression? _expression;

    internal ExpressionBuilder() { }

    public ExpressionBuilder Constant(string value)
    {
        _expression = new ConstantExpression(value);
        return this;
    }
    public ExpressionBuilder Inlined(string script)
    {
        _expression = new InlineScriptExpression(script);
        return this;
    }
    public ExpressionBuilder FromFile(string filePath)
    {
        _expression = new FileScriptExpression(filePath);
        return this;
    }

    public IExpression Build()
    {
        return _expression ?? throw new NullReferenceException();
    }
}