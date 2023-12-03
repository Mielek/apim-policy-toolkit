namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

public interface IMessageBody
{
    T As<T>(bool preserveContent = false);
}