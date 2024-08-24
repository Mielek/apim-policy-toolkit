using System.Collections.Immutable;
using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

// TODO graphql
public partial class HttpDataSourcePolicyBuilder
{
    private ICollection<XElement>? _httpRequest;
    private ICollection<XElement>? _backend;
    private ICollection<XElement>? _httpResponse;

    public HttpDataSourcePolicyBuilder HttpRequest(Action<HttpDataSourceHttpRequestBuilder> configurator)
    {
        var builder = new HttpDataSourceHttpRequestBuilder();
        configurator(builder);
        _httpRequest = builder.Build();
        return this;
    }
    public HttpDataSourcePolicyBuilder Backend(Action<HttpDataSourceHttpBackendBuilder> configurator)
    {
        var builder = new HttpDataSourceHttpBackendBuilder();
        configurator(builder);
        _backend = builder.Build();
        return this;
    }
    public HttpDataSourcePolicyBuilder HttpResponse(Action<HttpDataSourceHttpResponseBuilder> configurator)
    {
        var builder = new HttpDataSourceHttpResponseBuilder();
        configurator(builder);
        _httpResponse = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_httpRequest == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<XObject>();

        children.Add(new XElement("http-request", _httpRequest.ToArray()));

        if (_backend != null)
        {
            children.Add(new XElement("backend", _backend.ToArray()));
        }

        if (_httpResponse != null)
        {
            children.Add(new XElement("http-response", _httpResponse.ToArray()));
        }

        return new XElement("http-data-source", children.ToArray());
    }
}

public partial class HttpDataSourceHttpRequestBuilder
{
    private ImmutableArray<XElement>.Builder? _policies;

    public HttpDataSourceHttpRequestBuilder GetAuthorizationContext(Action<GetAuthorizationContextPolicyBuilder> configurator)
    {
        var builder = new GetAuthorizationContextPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    // public HttpDataSourceHttpRequestBuilder GetAuthorizationContext(Action<SetBackendServicePolicyBuilder> configurator)
    // {
    //     var builder = new SetBackendServicePolicyBuilder();
    //     configurator(builder);
    //     (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
    //     return this;
    // }

    public HttpDataSourceHttpRequestBuilder SetMethod(Action<SetMethodPolicyBuilder> configurator)
    {
        var builder = new SetMethodPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public HttpDataSourceHttpRequestBuilder SetUrl(string url)
    {
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(new XElement("set-url", url));
        return this;
    }

    public HttpDataSourceHttpRequestBuilder IncludeFragment(Action<IncludeFragmentPolicyBuilder> configurator)
    {
        var builder = new IncludeFragmentPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public HttpDataSourceHttpRequestBuilder SetHeader(Action<SetHeaderPolicyBuilder> configurator)
    {
        var builder = new SetHeaderPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public HttpDataSourceHttpRequestBuilder SetBody(Action<SetBodyPolicyBuilder> configurator)
    {
        var builder = new SetBodyPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public HttpDataSourceHttpRequestBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
    {
        var builder = new AuthenticationCertificatePolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public ICollection<XElement> Build()
    {
        if (_policies == null) throw new NullReferenceException();

        return _policies.ToImmutable();
    }
}

public partial class HttpDataSourceHttpBackendBuilder
{
    private ImmutableArray<XElement>.Builder? _policies;

    public HttpDataSourceHttpBackendBuilder ForwardRequest(Action<ForwardRequestPolicyBuilder> configurator)
    {
        var builder = new ForwardRequestPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public ICollection<XElement> Build()
    {
        if (_policies == null) throw new NullReferenceException();

        return _policies.ToImmutable();
    }
}

public partial class HttpDataSourceHttpResponseBuilder
{
    private ImmutableArray<XElement>.Builder? _policies;

    public HttpDataSourceHttpResponseBuilder SetBody(Action<SetBodyPolicyBuilder> configurator)
    {
        var builder = new SetBodyPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    // public HttpDataSourceHttpResponseBuilder XmlToJson(Action<XmlToJsonPolicyBuilder> configurator)
    // {
    //     var builder = new XmlToJsonPolicyBuilder();
    //     configurator(builder);
    //     (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
    //     return this;
    // }

    public HttpDataSourceHttpResponseBuilder FindAndReplace(Action<FindAndReplacePolicyBuilder> configurator)
    {
        var builder = new FindAndReplacePolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    // public HttpDataSourceHttpResponseBuilder PublishEvent(Action<PublishEventPolicyBuilder> configurator)
    // {
    //     var builder = new PublishEventPolicyBuilder();
    //     configurator(builder);
    //     (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
    //     return this;
    // }

    public HttpDataSourceHttpResponseBuilder IncludeFragment(Action<IncludeFragmentPolicyBuilder> configurator)
    {
        var builder = new IncludeFragmentPolicyBuilder();
        configurator(builder);
        (_policies ??= ImmutableArray.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public ICollection<XElement> Build()
    {
        if (_policies == null) throw new NullReferenceException();

        return _policies.ToImmutable();
    }
}