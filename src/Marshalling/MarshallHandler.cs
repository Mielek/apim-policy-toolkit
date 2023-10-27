namespace Mielek.Marshalling;

public abstract class MarshallerHandler<T> : IMarshallerHandler
{
    public Type Type => typeof(T);
    public abstract void Marshal(Marshaller marshaller, T element);
    public void Marshal(Marshaller marshaller, object element)
    {
        if (!Type.IsAssignableTo(element.GetType()))
            throw new Exception($"Marshaller for type \"{Type.FullName}\" cannot handle element of type \"{element.GetType().FullName}\"");

        Marshal(marshaller, (T)element);
    }
}