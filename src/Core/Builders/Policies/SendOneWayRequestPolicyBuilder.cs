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
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class SendOneWayRequestPolicyBuilder : BaseBuilder<SendOneWayRequestPolicyBuilder>
{
    public enum SendOneWayRequestMode { New, Copy }

    private SendOneWayRequestMode? _mode;
    private uint? _timeout;
    private IExpression<string>? _setUrl;
    [IgnoreBuilderField]
    private XElement? _setMethod;
    private IExpression<string>? _setBody;

    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _setHeaders;

    [IgnoreBuilderField]
    private XElement? _authenticationCertificate;

    public SendOneWayRequestPolicyBuilder SetMethod(Action<SetMethodPolicyBuilder> configurator)
    {
        var builder = new SetMethodPolicyBuilder();
        configurator(builder);
        _setMethod = builder.Build();
        return this;
    }

    public SendOneWayRequestPolicyBuilder SetHeader(Action<SetHeaderPolicyBuilder> configurator)
    {
        var builder = new SetHeaderPolicyBuilder();
        configurator(builder);
        (_setHeaders ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public SendOneWayRequestPolicyBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
    {
        var builder = new AuthenticationCertificatePolicyBuilder();
        configurator(builder);
        _authenticationCertificate = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_mode != SendOneWayRequestMode.Copy)
        {
            if (_setUrl == null) throw new PolicyValidationException("SetUrl is required for SendOneWayRequest");
            if (_setMethod == null) throw new PolicyValidationException("SetMethod is required for SendOneWayRequest");
        }

        var element = this.CreateElement("send-one-way-request");

        if (_mode != null)
        {
            element.Add(new XAttribute("mode", TranslateMode(_mode)));
        }
        if (_timeout != null)
        {
            element.Add(new XAttribute("timeout", _timeout));
        }
        if (_setMethod != null)
        {
            element.Add(_setMethod);
        }
        if (_setUrl != null)
        {
            element.Add(new XElement("set-url", _setUrl.GetXText()));
        }
        if (_setHeaders != null && _setHeaders.Count > 0)
        {
            element.Add(_setHeaders.ToArray());
        }
        if (_setBody != null)
        {
            element.Add(new XElement("set-body", _setBody.GetXText()));
        }
        if (_authenticationCertificate != null)
        {
            element.Add(_authenticationCertificate);
        }

        return element;
    }

    private static string TranslateMode(SendOneWayRequestMode? mode) => mode switch
    {
        SendOneWayRequestMode.Copy => "copy",
        SendOneWayRequestMode.New => "new",
        _ => throw new PolicyValidationException("Unknown mode for SendOneWayRequest"),
    };
}