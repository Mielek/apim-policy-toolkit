using Mielek.Expressions.Context;

using Newtonsoft.Json.Linq;

namespace Mielek.Testing.Expressions.Mocks;

public class MockBody : IMessageBody
{
    public string? Content { get; set; }

    public T As<T>(bool preserveContent = false)
    {
        if (Content == null) throw new NullReferenceException();

        if (typeof(T) == typeof(string)) return (T)(object)Content;
        if (typeof(T) == typeof(JObject)) return (T)(object)JObject.Parse(Content);

        throw new NotImplementedException();
    }
}