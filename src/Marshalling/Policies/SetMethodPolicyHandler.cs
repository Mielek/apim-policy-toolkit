using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetMethodPolicyHandler : MarshallerHandler<SetMethodPolicy>
{
    public override void Marshal(Marshaller marshaller, SetMethodPolicy element)
    {
        marshaller.Writer.WriteStartElement("set-method");
        marshaller.Writer.WriteExpressionAsAttribute("method", element.Method);
        marshaller.Writer.WriteEndElement();
    }
}