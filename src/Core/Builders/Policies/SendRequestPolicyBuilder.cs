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
public partial class SendRequestPolicyBuilder : BaseBuilder<SendRequestPolicyBuilder>
{
    public enum SendRequestMode { New, Copy }

    private string? _responseVariableName;
    private SendRequestMode? _mode;
    private uint? _timeout;
    private bool? _ignoreError;
    private IExpression<string>? _setUrl;
    [IgnoreBuilderField]
    private XElement? _setMethod;
    private IExpression<string>? _setBody;

    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _setHeaders;

    [IgnoreBuilderField]
    private XElement? _authenticationCertificate;

    public SendRequestPolicyBuilder SetMethod(Action<SetMethodPolicyBuilder> configurator)
    {
        var builder = new SetMethodPolicyBuilder();
        configurator(builder);
        _setMethod = builder.Build();
        return this;
    }

    public SendRequestPolicyBuilder SetHeader(Action<SetHeaderPolicyBuilder> configurator)
    {
        var builder = new SetHeaderPolicyBuilder();
        configurator(builder);
        (_setHeaders ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public SendRequestPolicyBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
    {
        var builder = new AuthenticationCertificatePolicyBuilder();
        configurator(builder);
        _authenticationCertificate = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_responseVariableName == null) throw new PolicyValidationException("ResponseVariableName is required for SendRequest");
        if (_mode != SendRequestMode.Copy)
        {
            if (_setUrl == null) throw new PolicyValidationException("SetUrl is required for SendRequest");
            if (_setMethod == null) throw new PolicyValidationException("SetMethod is required for SendRequest");
        }

        var element = CreateElement("send-request");

        if (_mode != null)
        {
            element.Add(new XAttribute("mode", TranslateMode(_mode)));
        }
        element.Add(new XAttribute("response-variable-name", _responseVariableName));
        if (_timeout != null)
        {
            element.Add(new XAttribute("timeout", _timeout));
        }
        if (_ignoreError != null)
        {
            element.Add(new XAttribute("ignore-error", _ignoreError));
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
    private static string TranslateMode(SendRequestMode? mode) => mode switch
    {
        SendRequestMode.Copy => "copy",
        SendRequestMode.New => "new",
        _ => throw new PolicyValidationException("Unknown mode for SendRequest"),
    };
}