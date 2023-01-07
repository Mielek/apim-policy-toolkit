using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class CacheLookupPolicyHandler : MarshallerHandler<CacheLookupPolicy>
{
    public override void Marshal(Marshaller marshaller, CacheLookupPolicy element)
    {
        marshaller.Writer.WriteStartElement("cache-lookup");

        marshaller.Writer.WriteAttribute("vary-by-developer", element.VaryByDeveloper);
        marshaller.Writer.WriteAttribute("vary-by-developer-group", element.VaryByDeveloperGroup);

        marshaller.Writer.WriteNullableAttribute("cache-type", TranslateCachingType(element.CatchingType));
        marshaller.Writer.WriteNullableAttribute("downstream-caching-type", TranslateDownstreamCachingType(element.DownstreamCachingType));
        marshaller.Writer.WriteNullableAttribute("must-revalidate", element.MustRevalidate);
        marshaller.Writer.WriteNullableAttribute("allow-private-response-caching", element.AllowPrivateResponseCaching);

        marshaller.Writer.WriteNullableElementCollection("vary-by-header", element.VaryByHeaders);
        marshaller.Writer.WriteNullableElementCollection("vary-by-query-parameter", element.VaryByQueryParameters);

        marshaller.Writer.WriteEndElement();
    }

    private static string? TranslateCachingType(CacheLookupCachingType? type) => type switch
    {
        CacheLookupCachingType.PreferExternal => "prefer-external",
        CacheLookupCachingType.External => "external",
        CacheLookupCachingType.Internal => "internal",
        _ => null,
    };

    private static string? TranslateDownstreamCachingType(CacheLookupDownstreamCachingType? type) => type switch
    {
        CacheLookupDownstreamCachingType.None => "none",
        CacheLookupDownstreamCachingType.Private => "private",
        CacheLookupDownstreamCachingType.Public => "public",
        _ => null,
    };
}