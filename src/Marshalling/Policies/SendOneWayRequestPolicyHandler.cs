using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SendOneWayRequestPolicyHandler : MarshallerHandler<SendOneWayRequestPolicy>
{
    public override void Marshal(Marshaller marshaller, SendOneWayRequestPolicy element)
    {
        marshaller.Writer.WriteStartElement("send-one-way-request");

        marshaller.Writer.WriteNullableAttribute("mode", TranslateMode(element.Mode));
        marshaller.Writer.WriteNullableAttribute("timeout", element.Timeout);

        marshaller.Writer.WriteNullableExpressionAsElement("set-url", element.SetUrl);
        element.SetMethod?.Accept(marshaller);

        if (element.SetHeaders != null && element.SetHeaders.Count > 0)
        {
            foreach (var setHeader in element.SetHeaders)
            {
                setHeader.Accept(marshaller);
            }
        }

        marshaller.Writer.WriteNullableExpressionAsElement("set-body", element.SetBody);

        element.AuthenticationCertificate?.Accept(marshaller);

        marshaller.Writer.WriteEndElement();
    }

    private static string? TranslateMode(SendOneWayRequestMode? mode) => mode switch
    {
        SendOneWayRequestMode.Copy => "copy",
        SendOneWayRequestMode.New => "new",
        _ => null,
    };
}