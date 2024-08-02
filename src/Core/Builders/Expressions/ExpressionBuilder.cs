using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;

public class ExpressionBuilder<T>
{

    public static ExpressionBuilder<T> Builder => new();

    private IExpression<T>? _expression;

    internal ExpressionBuilder() { }

    public ExpressionBuilder<T> Constant(T value)
    {
        _expression = new ConstantExpression<T>(value);
        return this;
    }

    public ExpressionBuilder<T> Lambda(Expression<T> lambda, [CallerArgumentExpression(nameof(lambda))] string? lambdaCode = null)
    {
        if (lambdaCode == null)
        {
            throw new Exception("Compiler services are not enabled");
        }

        var info = lambda.GetMethodInfo();
        ValidateParameters(info);
        _expression = new LambdaExpression<T>(lambdaCode);
        return this;
    }

    public ExpressionBuilder<T> Method(Expression<T> method, [CallerFilePath] string? sourceFilePath = null)
    {
        if (sourceFilePath == null)
        {
            throw new Exception("Compiler services are not enabled");
        }

        var info = method.GetMethodInfo();
        ValidateParameters(info);
        _expression = new MethodExpression<T>(info, sourceFilePath);
        return this;
    }

    public ExpressionBuilder<T> Function(Expression<T> func, [CallerArgumentExpression(nameof(func))] string? code = null, [CallerFilePath] string? sourceFilePath = null)
    {
        if (code == null || sourceFilePath == null)
        {
            throw new Exception("Compiler services are not enabled");
        }

        if (Regex.Match(code, @"\(?\s*[^)]*\s*\)?\s*=>\s*.+", RegexOptions.Singleline).Success)
        {
            return Lambda(func, code);
        }
        else
        {
            return Method(func, sourceFilePath);
        }
    }

    public IExpression<T> Build()
    {
        return _expression ?? throw new NullReferenceException();
    }

    private void ValidateParameters(MethodInfo method)
    {
        var parameters = method.GetParameters();
        if (parameters == null || parameters.Length > 1)
        {
            throw new Exception($"Expression should have only one parameter but have 0 or more then one");
        }

        var parameter = parameters[0];
        if (parameter.ParameterType != typeof(IExpressionContext))
        {
            throw new Exception($"Parameter should be of type \"IExpressionContext\" but is \"{parameter.ParameterType}\"");
        }

        if (parameter.Name != "context")
        {
            throw new Exception($"Context parameter should be named \"context\" but is \"{parameter.Name}\"");
        }
    }
}