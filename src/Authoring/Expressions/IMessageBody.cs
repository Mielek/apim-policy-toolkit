namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public interface IMessageBody
{
    T As<T>(bool preserveContent = false);
}