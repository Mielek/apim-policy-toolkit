using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class ForwardRequestPolicyBuilder : BaseBuilder<ForwardRequestPolicyBuilder>
{
    private uint? _timeout;
    private bool? _followRedirects;
    private bool? _bufferRequestBody;
    private bool? _bufferResponse;
    private bool? _failOnErrorStatusCode;

    public XElement Build()
    {
        var element = CreateElement("forward-request");

        if (_timeout != null)
        {
            element.Add(new XAttribute("timeout", _timeout));
        }
        if (_followRedirects != null)
        {
            element.Add(new XAttribute("follow-redirects", _followRedirects));
        }
        if (_bufferRequestBody != null)
        {
            element.Add(new XAttribute("buffer-request-body", _bufferRequestBody));
        }
        if (_bufferResponse != null)
        {
            element.Add(new XAttribute("buffer-response", _bufferResponse));
        }
        if (_failOnErrorStatusCode != null)
        {
            element.Add(new XAttribute("fail-on-error-status-code", _failOnErrorStatusCode));
        }

        return element;
    }
}