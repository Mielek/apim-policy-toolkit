using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class IpFilterPolicyHandler : MarshallerHandler<IpFilterPolicy>
{
    public override void Marshal(Marshaller marshaller, IpFilterPolicy element)
    {
        marshaller.Writer.WriteStartElement("ip-filter");

        marshaller.Writer.WriteAttribute("action", TranslateAction(element.Action));

        foreach (var ipFilterValue in element.Values)
        {
            switch (ipFilterValue)
            {
                case IpFilterAddress address:
                    MarshallAddress(marshaller, address);
                    break;
                case IpFilterAddressRange range:
                    MarshallAddressRange(marshaller, range);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        marshaller.Writer.WriteEndElement();
    }

    private void MarshallAddressRange(Marshaller marshaller, IpFilterAddressRange range)
    {
        marshaller.Writer.WriteStartElement("address-range");
        marshaller.Writer.WriteAttribute("from", range.FromIp);
        marshaller.Writer.WriteAttribute("to", range.ToIp);
        marshaller.Writer.WriteEndElement();
    }

    private void MarshallAddress(Marshaller marshaller, IpFilterAddress address)
    {
        marshaller.Writer.WriteElement("address", address.Ip);
    }

    private string TranslateAction(IpFilterAction action) => action switch
    {
        IpFilterAction.Allow => "allow",
        IpFilterAction.Forbid => "forbid",
        _ => throw new NotImplementedException(),
    };
}