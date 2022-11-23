using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class BasePolicyHandler : MarshallerHandler<BasePolicy>
{
    public override void Marshal(Marshaller marshaller, BasePolicy element)
    {
        marshaller.Writer.WriteElement("base");
    }
}