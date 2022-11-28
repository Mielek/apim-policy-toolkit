namespace Mielek.Marshalling;

public interface IMarshallerHandler
{
    Type Type { get; }
    void Marshal(Marshaller marshaller, object element);
}