namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class XmlToJsonPolicyBuilder
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
        if (_kind == null) throw new NullReferenceException();
        if (_apply == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<XObject>();

        children.Add(_kind.GetXAttribute("kind"));
        children.Add(_apply.GetXAttribute("apply"));

        if (_considerAcceptHeader != null)
        {
            children.Add(_considerAcceptHeader.GetXAttribute("consider-accept-header"));
        }

        return new XElement("xml-to-json", children.ToArray());
    }

    private string TranslateKind(XmlToJsonKind kind)
    {
        return kind switch
        {
            XmlToJsonKind.JavascriptFriendly => "javascript-friendly",
            XmlToJsonKind.Direct => "direct",
            _ => throw new NotImplementedException(),
        };
    }

    private string TranslateAccept(XmlToJsonApply kind)
    {
        return kind switch
        {
            XmlToJsonApply.Always => "always",
            XmlToJsonApply.ContentTypeXml => "content-type-xml",
            _ => throw new NotImplementedException(),
        };
    }
}