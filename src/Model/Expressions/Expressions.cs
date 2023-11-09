using System.Reflection;

namespace Mielek.Model.Expressions;

public sealed record ConstantExpression<T>(T Value) : Visitable<ConstantExpression<T>>, IExpression<T>;
public sealed record InlineExpression<T>(string Expression) : Visitable<InlineExpression<T>>, IExpression<T>;
public sealed record LambdaExpression<T>(MethodInfo LambdaInfo, string Code) : Visitable<LambdaExpression<T>>, IExpression<T>;
public sealed record MethodExpression<T>(MethodInfo MethodInfo, string FilePath) : Visitable<MethodExpression<T>>, IExpression<T>;
