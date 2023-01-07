using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class WaitPolicyHandler : MarshallerHandler<WaitPolicy>
{
    public override void Marshal(Marshaller marshaller, WaitPolicy element)
    {
        marshaller.Writer.WriteStartElement("wait");

        marshaller.Writer.WriteNullableAttribute("for", TranslateFor(element.For));

        foreach (var policy in element.Policies)
        {
            policy.Accept(marshaller);
        }

        marshaller.Writer.WriteEndElement();
    }

    private static string? TranslateFor(WaitFor? waitFor) => waitFor switch
    {
        WaitFor.All => "all",
        WaitFor.Any => "any",
        _ => null,
    };
}