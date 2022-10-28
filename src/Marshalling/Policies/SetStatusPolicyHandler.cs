
using System.Xml;

using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetStatusPolicyHandler : MarshallerHandler<SetStatusPolicy>
{
    public override void Marshal(Marshaller marshaller, SetStatusPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-status");
        marshaller.Writer.WriteExpressionAsAttribute("code", element.Code);
        if(element.Reason != null) {
            marshaller.Writer.WriteExpressionAsAttribute("reason", element.Reason);
        }
        marshaller.Writer.WriteEndElement();
    }
}