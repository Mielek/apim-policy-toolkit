using Mielek.Model;
using Mielek.Model.Policies;

namespace Mielek.Marshalling;
public sealed class PolicyDocumentHandler : MarshallerHandler<PolicyDocument>
{
    public override void Marshal(Marshaller marshaler, PolicyDocument document)
    {
        marshaler.Writer.WriteStartElement("policies");
        this.WriteSection(marshaler, "inbound", document.Inbound);
        this.WriteSection(marshaler, "backend", document.Backend);
        this.WriteSection(marshaler, "outbound", document.Outbound);
        this.WriteSection(marshaler, "on-error", document.OnError);
        marshaler.Writer.WriteEndElement();
    }


    private void WriteSection(Marshaller marshaler, string section, ICollection<IPolicy>? policies)
    {
        if (policies != null && policies.Count > 0)
        {
            marshaler.Writer.WriteStartElement(section);
            foreach (var policy in policies)
            {
                policy.Accept(marshaler);
            }
            marshaler.Writer.WriteEndElement();
        }
        else
        {
            marshaler.Writer.WriteElement(section);
        }

    }
}