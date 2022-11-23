
using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class CheckHeaderPolicyHandler : MarshallerHandler<CheckHeaderPolicy>
{
    public override void Marshal(Marshaller marshaller, CheckHeaderPolicy element)
    {
        marshaller.Writer.WriteStartElement("check-header");

        marshaller.Writer.WriteExpressionAsAttribute("name", element.Name);
        marshaller.Writer.WriteExpressionAsAttribute("failed-check-httpcode", element.FailedCheckHttpCode);
        marshaller.Writer.WriteExpressionAsAttribute("failed-check-error-message", element.FailedCheckErrorMessage);
        marshaller.Writer.WriteExpressionAsAttribute("ignore-case", element.IgnoreCase);

        foreach (var value in element.Values)
        {
            marshaller.Writer.WriteExpressionAsElement("value", value);
        }

        marshaller.Writer.WriteEndElement();
    }
}