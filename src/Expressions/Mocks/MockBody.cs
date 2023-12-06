using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

using Newtonsoft.Json.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockBody : IMessageBody
{
    public string? Content { get; set; }

    public bool Consumed { get; private set; } = false;

    public T As<T>(bool preserveContent = false)
    {
        if (Content == null) throw new NullReferenceException();

        Consumed = !preserveContent;

        if (typeof(T) == typeof(string)) return (T)(object)Content;
        if (typeof(T) == typeof(JObject)) return (T)(object)JObject.Parse(Content);
        if (typeof(T) == typeof(XElement)) return (T)(object)XElement.Parse(Content);
        if (typeof(T) == typeof(XDocument)) return (T)(object)XDocument.Parse(Content);

        throw new NotImplementedException();
    }
}