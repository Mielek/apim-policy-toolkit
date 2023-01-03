namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ValidateJwtPolicyBuilder
    {
        string? _headerName;
        string? _queryParameterName;
        string? _tokenValue;
        ushort? _failedValidationHttpCode;
        string? _failedValidationErrorMessage;
        bool? _requireExpirationTime;
        string? _requireScheme;
        bool? _requireSignedTokens;
        uint? _clockSkew;
        string? _outputTokenVariableName;
        
        [IgnoreBuilderField]
        ValidateJwtOpenIdConfig? _openIdConfig;
        ImmutableList<string>.Builder? _issuerSigningKeys;
        ImmutableList<string>.Builder? _decryptionKeys;
        ImmutableList<IExpression<string>>.Builder? _audiences;
        ImmutableList<string>.Builder? _issuers;

        [IgnoreBuilderField]
        ImmutableList<ValidateJwtClaim>.Builder? _requiredClaims;

        public ValidateJwtPolicyBuilder RequiredClaim(Action<ValidateJwtClaimBuilder> configurator)
        {
            var builder = new ValidateJwtClaimBuilder();
            configurator(builder);
            (_requiredClaims ??= ImmutableList.CreateBuilder<ValidateJwtClaim>()).Add(builder.Build());
            return this;
        }

        public ValidateJwtPolicyBuilder OpenIdConfig(string url)
        {
            _openIdConfig = new ValidateJwtOpenIdConfig(url);
            return this;
        }

        public ValidateJwtPolicy Build()
        {
            return new ValidateJwtPolicy(
                _headerName,
                _queryParameterName,
                _tokenValue,
                _failedValidationHttpCode,
                _failedValidationErrorMessage,
                _requireExpirationTime,
                _requireScheme,
                _requireSignedTokens,
                _clockSkew,
                _outputTokenVariableName,
                _openIdConfig,
                _issuerSigningKeys?.ToImmutable(),
                _decryptionKeys?.ToImmutable(),
                _audiences?.ToImmutable(),
                _issuers?.ToImmutable());
        }
    }

    [GenerateBuilderSetters]
    public partial class ValidateJwtClaimBuilder
    {
        string? _name;
        ImmutableList<string>.Builder? _values;
        ValidateJwtClaimMatch? _match;
        string? _separator;

        public ValidateJwtClaim Build()
        {
            if (_name == null) throw new NullReferenceException();
            if (_values == null) throw new NullReferenceException();

            return new ValidateJwtClaim(_name, _values.ToImmutable(), _match, _separator);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder ValidateJwt(Action<ValidateJwtPolicyBuilder> configurator)
        {
            var builder = new ValidateJwtPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}