using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SetVariablePolicyHandler : MarshallerHandler<SetVariablePolicy>
{
    public override void Marshal(Marshaller marshaller, SetVariablePolicy element)
    {
        marshaller.Writer.WriteStartElement("set-variable");

        marshaller.Writer.WriteAttribute("name", element.Name);
        marshaller.Writer.WriteExpressionAsAttribute("value", element.Value);

        marshaller.Writer.WriteEndElement();
    }
}