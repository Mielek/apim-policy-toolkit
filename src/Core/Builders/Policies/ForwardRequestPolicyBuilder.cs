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
    private readonly uint? _timeout;
    private readonly bool? _followRedirects;
    private readonly bool? _bufferRequestBody;
    private readonly bool? _bufferResponse;
    private readonly bool? _failOnErrorStatusCode;

    public XElement Build()
    {
        var element = this.CreateElement("forward-request");

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