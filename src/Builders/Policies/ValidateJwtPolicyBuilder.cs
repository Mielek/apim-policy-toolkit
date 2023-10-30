namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ValidateJwtPolicyBuilder
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
        
        [IgnoreBuilderField]
        private ValidateJwtOpenIdConfig? _openIdConfig;
        private ImmutableList<string>.Builder? _issuerSigningKeys;
        private ImmutableList<string>.Builder? _decryptionKeys;
        private ImmutableList<IExpression<string>>.Builder? _audiences;
        private ImmutableList<string>.Builder? _issuers;

        [IgnoreBuilderField]
        private ImmutableList<ValidateJwtClaim>.Builder? _requiredClaims;

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
        private string? _name;
        private ImmutableList<string>.Builder? _values;
        private ValidateJwtClaimMatch? _match;
        private string? _separator;

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