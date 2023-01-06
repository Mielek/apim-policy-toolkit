
using Mielek.Model.Policies;

namespace Mielek.Marshalling.Policies;

public class EmitMetricPolicyHandler : MarshallerHandler<EmitMetricPolicy>
{
    public override void Marshal(Marshaller marshaller, EmitMetricPolicy element)
    {
        marshaller.Writer.WriteStartElement("emit-metric");

        marshaller.Writer.WriteAttribute("name", element.Name);
        marshaller.Writer.WriteNullableExpressionAsAttribute("value", element.Value);
        marshaller.Writer.WriteNullableAttribute("namespace", element.Namespace);

        foreach (var dimension in element.Dimensions)
        {
            marshaller.Writer.WriteStartElement("dimension");

            marshaller.Writer.WriteAttribute("name", dimension.Name);
            marshaller.Writer.WriteNullableExpressionAsAttribute("value", dimension.Value);

            marshaller.Writer.WriteEndElement();
        }

        marshaller.Writer.WriteEndElement();
    }
}