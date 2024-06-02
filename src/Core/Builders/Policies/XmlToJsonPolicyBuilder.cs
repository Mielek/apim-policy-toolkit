using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class XmlToJsonPolicyBuilder : BaseBuilder<XmlToJsonPolicyBuilder>
{
    public enum XmlToJsonKind { JavascriptFriendly, Direct }

    public enum XmlToJsonApply { Always, ContentTypeXml }

    private IExpression<string>? _kind;
    private IExpression<string>? _apply;
    private IExpression<bool>? _considerAcceptHeader;

    public XmlToJsonPolicyBuilder Kind(XmlToJsonKind kind)
    {
        _kind = ExpressionBuilder<string>.Builder.Constant(TranslateKind(kind)).Build();
        return this;
    }

    public XmlToJsonPolicyBuilder Apply(XmlToJsonApply apply)
    {
        _kind = ExpressionBuilder<string>.Builder.Constant(TranslateAccept(apply)).Build();
        return this;
    }

    public XElement Build()
    {
        if (_kind == null) throw new PolicyValidationException("Kind is required for XmlToJson");
        if (_apply == null) throw new PolicyValidationException("Apply is required for XmlToJson");

        var element = this.CreateElement("xml-to-json");

        element.Add(_kind.GetXAttribute("kind"));
        element.Add(_apply.GetXAttribute("apply"));

        if (_considerAcceptHeader != null)
        {
            element.Add(_considerAcceptHeader.GetXAttribute("consider-accept-header"));
        }

        return element;
    }

    private string TranslateKind(XmlToJsonKind kind)
    {
        return kind switch
        {
            XmlToJsonKind.JavascriptFriendly => "javascript-friendly",
            XmlToJsonKind.Direct => "direct",
            _ => throw new PolicyValidationException("Unknown kind for XmlToJson policy"),
        };
    }

    private string TranslateAccept(XmlToJsonApply kind)
    {
        return kind switch
        {
            XmlToJsonApply.Always => "always",
            XmlToJsonApply.ContentTypeXml => "content-type-xml",
            _ => throw new PolicyValidationException("Unknown apply for XmlToJson policy"),
        };
    }
}