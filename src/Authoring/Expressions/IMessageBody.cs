namespace Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public interface IMessageBody
{
    T As<T>(bool preserveContent = false);
    IDictionary<string, IList<string>> AsFormUrlEncodedContent(bool preserveContent = false);
}