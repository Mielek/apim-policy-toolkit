using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class ForwardRequestPolicyHandler : MarshallerHandler<ForwardRequestPolicy>
{
    public override void Marshal(Marshaller marshaller, ForwardRequestPolicy element)
    {
        marshaller.Writer.WriteStartElement("forward-request");
        marshaller.Writer.WriteNullableAttribute("timeout", element.Timeout);
        marshaller.Writer.WriteNullableAttribute("follow-redirects", element.FollowRedirects);
        marshaller.Writer.WriteNullableAttribute("buffer-request-body", element.BufferRequestBody);
        marshaller.Writer.WriteNullableAttribute("buffer-response", element.BufferResponse);
        marshaller.Writer.WriteNullableAttribute("fail-on-error-status-code", element.FailOnErrorStatusCode);
        marshaller.Writer.WriteEndElement();
    }
}