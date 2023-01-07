using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class TracePolicyHandler : MarshallerHandler<TracePolicy>
{
    public override void Marshal(Marshaller marshaller, TracePolicy element)
    {
        marshaller.Writer.WriteStartElement("trace");

        marshaller.Writer.WriteAttribute("source", element.Source);
        marshaller.Writer.WriteNullableAttribute("severity", TranslateSeverity(element.Severity));

        marshaller.Writer.WriteExpressionAsElement("message", element.Message);

        if (element.Metadatas != null && element.Metadatas.Count > 0)
        {
            foreach (var metadata in element.Metadatas)
            {
                marshaller.Writer.WriteStartElement("metadata");
                marshaller.Writer.WriteAttribute("name", metadata.Name);
                marshaller.Writer.WriteExpressionAsAttribute("value", metadata.Value);
            }
        }

        marshaller.Writer.WriteEndElement();
    }

    private static string? TranslateSeverity(TraceSeverity? severity) => severity switch
    {
        TraceSeverity.Verbose => "verbose",
        TraceSeverity.Information => "information",
        TraceSeverity.Error => "error",
        _ => null,
    };
}