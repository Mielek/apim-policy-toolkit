namespace Mielek.Marshalling;

public abstract class MarshallerHandler<T> : IMarshallerHandler
{
    public abstract void Marshal(Marshaller marshaller, T element);
    public void Marshal(Marshaller marshaller, object element)
    {
        if (typeof(T) != element.GetType()) throw new Exception();

        Marshal(marshaller, (T)element);
    }
}