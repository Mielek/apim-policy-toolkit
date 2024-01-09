namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class SetBodyPolicyBuilder
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
        _ => throw new Exception(),
    };

    public SetBodyPolicyBuilder XsiNil(XsiNilType xsiNil)
    {
        return XsiNil(TranslateXsiNil(xsiNil));
    }

    private string TranslateXsiNil(XsiNilType xsiNil) => xsiNil switch
    {
        XsiNilType.Blank => "blank",
        XsiNilType.Null => "null",
        _ => throw new Exception(),
    };

    public XElement Build()
    {
        if (_body == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        if (_template != null)
        {
            children.Add(_template.GetXAttribute("template"));
        }
        if (_xsiNil != null)
        {
            children.Add(_xsiNil.GetXAttribute("xsi-nil"));
        }

        children.Add(_body.GetXText());

        return new XElement("set-body", children.ToArray());
    }
}