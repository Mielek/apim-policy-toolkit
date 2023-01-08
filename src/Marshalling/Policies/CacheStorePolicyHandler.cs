using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class CacheStorePolicyHandler : MarshallerHandler<CacheStorePolicy>
{
    public override void Marshal(Marshaller marshaller, CacheStorePolicy element)
    {
        marshaller.Writer.WriteStartElement("cache-store");

        marshaller.Writer.WriteAttribute("duration", element.Duration);
        marshaller.Writer.WriteNullableAttribute("client-id", element.CacheResponse);

        marshaller.Writer.WriteEndElement();
    }
}