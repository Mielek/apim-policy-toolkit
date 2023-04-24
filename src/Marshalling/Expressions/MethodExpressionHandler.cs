using Mielek.Model.Expressions;

namespace Mielek.Marshalling.Expressions;

public class MethodExpressionHandler<T> : MarshallerHandler<MethodExpression<T>>
{
    public override void Marshal(Marshaller marshaller, MethodExpression<T> element)
    {
        var lambda = element.Fuc.Method.GetCustomAttributes(typeof(LambdaExpressionAttribute), false);
        var method = element.Fuc.Method.GetCustomAttributes(typeof(MethodExpressionAttribute), false);

        if (method.Length > 0)
        {
            var key = element.MethodName;
            var found = marshaller.Options.MethodLibrary.TryGetValue(key, out var source);
            if (found)
            {
                marshaller.Writer.WriteRawString($"@({source})");
            }
            else
            {
                throw new Exception($"method {key}");
            }
        }
        else if (lambda.Length > 0)
        {
            var key = ((LambdaExpressionAttribute) lambda[0]).Name;
            var found = marshaller.Options.MethodLibrary.TryGetValue(((LambdaExpressionAttribute) lambda[0]).Name, out var source);
            if (found)
            {
                marshaller.Writer.WriteRawString($"@({source})");
            }
            else
            {
                throw new Exception($"lambda {key}");
            }
        }
        else
        {
            throw new Exception($"Possibly [MethodExpression] or [LambdaExpression] attribute missing");
        }
    }
}
