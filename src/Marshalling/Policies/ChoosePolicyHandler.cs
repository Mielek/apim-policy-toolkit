using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class ChoosePolicyHandler : MarshallerHandler<ChoosePolicy>
{
    public override void Marshal(Marshaller marshaller, ChoosePolicy element)
    {
        marshaller.Writer.WriteStartElement("choose");

        foreach (var when in element.Whens)
        {
            marshaller.Writer.WriteStartElement("when");
            marshaller.Writer.WriteAttribute("condition", when.Condition);
            foreach (var policy in when.Policies)
            {
                policy.Accept(marshaller);
            }
            marshaller.Writer.WriteEndElement();
        }

        if (element.Otherwise != null)
        {
            marshaller.Writer.WriteStartElement("otherwise");
            foreach (var policy in element.Otherwise)
            {
                policy.Accept(marshaller);
            }
            marshaller.Writer.WriteEndElement();
        }

        marshaller.Writer.WriteEndElement();
    }
}