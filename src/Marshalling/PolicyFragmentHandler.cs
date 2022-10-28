using System.Xml;

using Mielek.Model;

namespace Mielek.Marshalling;
public sealed class PolicyFragmentHandler : MarshallerHandler<PolicyFragment>
{
    public override void Marshal(Marshaller marshaler, PolicyFragment fragment)
    {
        marshaler.Writer.WriteStartElement("fragment");
        foreach (var policy in fragment.Policies)
        {
            policy.Accept(marshaler);
        }
        marshaler.Writer.WriteEndElement();
    }

}