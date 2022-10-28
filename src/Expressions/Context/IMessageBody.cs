namespace Mielek.Expressions.Context;

public interface IMessageBody
{
    T As<T>(bool preserveContent = false);
}