using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class SetBodyPolicyBuilder : BaseBuilder<SetBodyPolicyBuilder>
{
    public enum BodyTemplate { Liquid }
    public enum XsiNilType { Blank, Null }

    private IExpression<string>? _body;
    private IExpression<string>? _template;
    private IExpression<string>? _xsiNil;

    public SetBodyPolicyBuilder Template(BodyTemplate template)
    {
        return Template(TranslateTemplate(template));
    }

    private string TranslateTemplate(BodyTemplate template) => template switch
    {
        BodyTemplate.Liquid => "liquid",
        _ => throw new PolicyValidationException("Unknown template type for SetBody"),
    };

    public SetBodyPolicyBuilder XsiNil(XsiNilType xsiNil)
    {
        return XsiNil(TranslateXsiNil(xsiNil));
    }

    private string TranslateXsiNil(XsiNilType xsiNil) => xsiNil switch
    {
        XsiNilType.Blank => "blank",
        XsiNilType.Null => "null",
        _ => throw new PolicyValidationException("Unknown xsi-nil type for SetBody"),
    };

    public XElement Build()
    {
        if (_body == null) throw new PolicyValidationException("Body is required for SetBody");

        var element = CreateElement("set-body");
        if (_template != null)
        {
            element.Add(_template.GetXAttribute("template"));
        }
        if (_xsiNil != null)
        {
            element.Add(_xsiNil.GetXAttribute("xsi-nil"));
        }

        element.Add(_body.GetXText());

        return element;
    }
}