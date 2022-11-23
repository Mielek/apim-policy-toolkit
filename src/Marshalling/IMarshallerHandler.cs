namespace Mielek.Marshalling;

public interface IMarshallerHandler
{
    void Marshal(Marshaller marshaller, object element);
}