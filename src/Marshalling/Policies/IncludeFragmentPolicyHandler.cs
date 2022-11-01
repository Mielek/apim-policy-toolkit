using System.Xml;

using Mielek.Marshalling;
using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class IncludeFragmentPolicyHandler : MarshallerHandler<IncludeFragmentPolicy>
{
    public override void Marshal(Marshaller marshaller, IncludeFragmentPolicy element)
    {
        marshaller.Writer.WriteStartElement("include-fragment");
        marshaller.Writer.WriteAttribute("fragment-id", element.FragmentId);
        marshaller.Writer.WriteEndElement();
    }
}