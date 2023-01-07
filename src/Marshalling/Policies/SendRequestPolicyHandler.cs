using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class SendRequestPolicyHandler : MarshallerHandler<SendRequestPolicy>
{
    public override void Marshal(Marshaller marshaller, SendRequestPolicy element)
    {
        marshaller.Writer.WriteStartElement("send-request");

        marshaller.Writer.WriteNullableAttribute("mode", TranslateMode(element.Mode));
        marshaller.Writer.WriteAttribute("response-variable-name", element.ResponseVariableName);
        marshaller.Writer.WriteNullableAttribute("timeout", element.Timeout);
        marshaller.Writer.WriteNullableAttribute("ignore-error", element.IgnoreError);

        marshaller.Writer.WriteNullableExpressionAsElement("set-url", element.SetUrl);
        marshaller.Writer.WriteNullableExpressionAsElement("set-method", element.SetMethod);
        
        if(element.SetHeaders != null && element.SetHeaders.Count > 0)
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

    private static string? TranslateMode(SendRequestMode? mode) => mode switch
    {
        SendRequestMode.Copy => "copy",
        SendRequestMode.New => "new",
        _ => null,
    };
}