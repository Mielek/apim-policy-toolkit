using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class ValidateJwtPolicyBuilder : BaseBuilder<ValidateJwtPolicyBuilder>
{
    private string? _headerName;
    private string? _queryParameterName;
    private string? _tokenValue;
    private ushort? _failedValidationHttpCode;
    private string? _failedValidationErrorMessage;
    private bool? _requireExpirationTime;
    private string? _requireScheme;
    private bool? _requireSignedTokens;
    private uint? _clockSkew;
    private string? _outputTokenVariableName;
    private string? _openIdConfigUrl;
    private ImmutableList<string>.Builder? _issuerSigningKeys;
    private ImmutableList<string>.Builder? _decryptionKeys;
    private ImmutableList<IExpression<string>>.Builder? _audiences;
    private ImmutableList<string>.Builder? _issuers;

    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _requiredClaims;

    public ValidateJwtPolicyBuilder RequiredClaim(Action<ValidateJwtClaimBuilder> configurator)
    {
        var builder = new ValidateJwtClaimBuilder();
        configurator(builder);
        (_requiredClaims ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public XElement Build()
    {
        var element = CreateElement("validate-jwt");

        if (_headerName != null)
        {
            element.Add(new XAttribute("header-name", _headerName));
        }
        if (_queryParameterName != null)
        {
            element.Add(new XAttribute("query-parameter-name", _queryParameterName));
        }
        if (_tokenValue != null)
        {
            element.Add(new XAttribute("token-value", _tokenValue));
        }
        if (_failedValidationHttpCode != null)
        {
            element.Add(new XAttribute("failed-validation-httpcode", _failedValidationHttpCode));
        }
        if (_failedValidationErrorMessage != null)
        {
            element.Add(new XAttribute("failed-validation-error-message", _failedValidationErrorMessage));
        }
        if (_requireExpirationTime != null)
        {
            element.Add(new XAttribute("require-expiration-time", _requireExpirationTime));
        }
        if (_requireScheme != null)
        {
            element.Add(new XAttribute("require-scheme", _requireScheme));
        }
        if (_requireSignedTokens != null)
        {
            element.Add(new XAttribute("require-signed-tokens", _requireSignedTokens));
        }
        if (_clockSkew != null)
        {
            element.Add(new XAttribute("clock-skew", _clockSkew));
        }

        if (_outputTokenVariableName != null)
        {
            element.Add(new XAttribute("output-token-variable-name", _outputTokenVariableName));
        }

        if (_issuerSigningKeys != null && _issuerSigningKeys.Count > 0)
        {
            element.Add(new XElement("issuer-signing-keys", _issuerSigningKeys.Select(i => new XElement("key", i)).ToArray()));
        }
        if (_decryptionKeys != null && _decryptionKeys.Count > 0)
        {
            element.Add(new XElement("decryption-keys", _decryptionKeys.Select(i => new XElement("key", i)).ToArray()));
        }
        if (_audiences != null && _audiences.Count > 0)
        {
            element.Add(new XElement("audiences", _audiences.Select(i => new XElement("audience", i.GetXText())).ToArray()));
        }
        if (_issuers != null && _issuers.Count > 0)
        {
            element.Add(new XElement("issuers", _issuers.Select(i => new XElement("issuer", i)).ToArray()));
        }
        if (_requiredClaims != null && _requiredClaims.Count > 0)
        {
            element.Add(new XElement("required-claims", _requiredClaims.ToArray()));
        }

        return element;
    }
}

[GenerateBuilderSetters]
public partial class ValidateJwtClaimBuilder
{
    public enum ClaimMatch { All, Any }
    private string? _name;
    private ImmutableList<string>.Builder? _values;
    private ClaimMatch? _match;
    private string? _separator;

    public XElement Build()
    {
        if (_name == null) throw new NullReferenceException();
        if (_values == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<XObject>();
        children.Add(new XAttribute("name", _name));
        if (_match != null)
        {
            children.Add(new XAttribute("match", TranslateClaimMatch(_match)));
        }
        if (_separator != null)
        {
            children.Add(new XAttribute("separator", _separator));
        }

        children.AddRange(_values.Select(v => new XElement("value", v)));

        return new XElement("claim", children.ToArray());
    }


    private string TranslateClaimMatch(ClaimMatch? match) => match switch
    {
        ClaimMatch.All => "all",
        ClaimMatch.Any => "any",
        _ => throw new NotImplementedException(),
    };
}