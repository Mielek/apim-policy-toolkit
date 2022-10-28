using System.Xml;

namespace Mielek.Marshalling;

public abstract class MarshallerHandler<T> : IMarshallerHandler
{
    public abstract void Marshal(Marshaller marshaller, T element);
    public void Marshal(Marshaller marshaller, object element) {
        this.Marshal(marshaller, (T)element);
    }
}