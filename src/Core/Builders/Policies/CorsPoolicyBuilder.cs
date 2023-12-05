namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;


    [GenerateBuilderSetters]
    public partial class CorsPolicyBuilder
    {
        private IExpression<bool>? _allowCredentials;
        private IExpression<bool>? _terminateUnmatchedRequest;
        private ImmutableList<string>.Builder? _allowedOrigins;

        private IExpression<int>? _preflightResultMaxAge;
        private ImmutableList<string>.Builder? _allowedMethods;

        private ImmutableList<string>.Builder? _allowedHeaders;
        private ImmutableList<string>.Builder? _exposeHeaders;

        public CorsPolicyBuilder AllowedOrigin(Uri uri)
            => AllowedOrigin(uri.ToString());

        public CorsPolicyBuilder AllowedMethod(HttpMethod method)
            => AllowedMethod(method.Method);

        public XElement Build()
        {
            if (_allowedOrigins == null) throw new NullReferenceException();
            if (_allowedHeaders == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<XObject>();

            if (_allowCredentials != null)
            {
                children.Add(_allowCredentials.GetXAttribute("allow-credentials"));
            }
            if (_terminateUnmatchedRequest != null)
            {
                children.Add(_terminateUnmatchedRequest.GetXAttribute("terminate-unmatched-request"));
            }

            var allowedOrigins = new XElement("allowed-origins", _allowedOrigins.ToImmutable().Select(origin => new XElement("origin", origin)).ToArray());
            children.Add(allowedOrigins);

            if (_allowedMethods != null && _allowedMethods.Count > 0)
            {
                var allowedMethodsChildren = ImmutableArray.CreateBuilder<XObject>();
                if (_preflightResultMaxAge != null)
                {
                    allowedMethodsChildren.Add(_preflightResultMaxAge.GetXAttribute("preflight-result-max-age"));
                }
                allowedMethodsChildren.AddRange(_allowedMethods.ToImmutable().Select(method => new XElement("method", method)));

                return new XElement("allowed-methods", allowedMethodsChildren.ToArray());
            }

            var allowedHeaders = new XElement("allowed-headers", _allowedHeaders.ToImmutable().Select(header => new XElement("header", header)).ToArray());
            children.Add(allowedHeaders);

            if (_exposeHeaders != null && _exposeHeaders.Count > 0)
            {
                var exposeHeaders = new XElement("expose-headers", _exposeHeaders.ToImmutable().Select(header => new XElement("header", header)).ToArray());
                children.Add(exposeHeaders);
            }

            return new XElement("cors", children.ToArray());
        }
    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder Cors(Action<CorsPolicyBuilder> configurator)
        {
            var builder = new CorsPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}