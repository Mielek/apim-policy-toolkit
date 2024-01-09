using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class TypeUsedAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Rules.TypeUsed.All;

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression,
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxKind.ElementAccessExpression,
                SyntaxKind.ObjectCreationExpression,
                SyntaxKind.ObjectInitializerExpression,
                SyntaxKind.AnonymousObjectCreationExpression);
    }

    private const string LibraryAttribute = "Mielek.Azure.ApiManagement.PolicyToolkit.Attributes.LibraryAttribute";

    private readonly static IReadOnlySet<string> AllowedTypes = new HashSet<string>()
    {
        #region Expression

        #region  mslib
        "System.Array",
        "System.BitConverter",
        "System.Boolean",
        "System.Byte",
        "System.Char",
        "System.Collections.Generic.Dictionary<TKey, TValue>",
        "System.Collections.Generic.HashSet<T>",
        "System.Collections.Generic.ICollection<T>",
        "System.Collections.Generic.IDictionary<TKey, TValue>",
        "System.Collections.Generic.IEnumerable<T>",
        "System.Collections.Generic.IEnumerator<T>",
        "System.Collections.Generic.IList<T>",
        "System.Collections.Generic.IReadOnlyCollection<T>",
        "System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>",
        "System.Collections.Generic.ISet<T>",
        "System.Collections.Generic.KeyValuePair<TKey, TValue>",
        "System.Collections.Generic.List<T>",
        "System.Collections.Generic.Queue<T>",
        "System.Collections.Generic.Stack<T>",
        "System.Convert",
        "System.DateTime",
        "System.DateTimeKind",
        "System.DateTimeOffset",
        "System.Decimal",
        "System.Double",
        "System.Enum",
        "System.Exception",
        "System.Guid",
        "System.Int16",
        "System.Int32",
        "System.Int64",
        "System.IO.StringReader",
        "System.IO.StringWriter",
        "System.Linq.Enumerable",
        "System.Math",
        "System.MidpointRounding",
        "System.Net.IPAddress",
        "System.Net.WebUtility",
        "System.Nullable",
        "System.Object",
        "System.Random",
        "System.SByte",
        "System.Security.Cryptography.AsymmetricAlgorithm",
        "System.Security.Cryptography.CipherMode",
        "System.Security.Cryptography.HashAlgorithm",
        "System.Security.Cryptography.HashAlgorithmName",
        "System.Security.Cryptography.HMAC",
        "System.Security.Cryptography.HMACMD5",
        "System.Security.Cryptography.HMACSHA1",
        "System.Security.Cryptography.HMACSHA256",
        "System.Security.Cryptography.HMACSHA384",
        "System.Security.Cryptography.HMACSHA512",
        "System.Security.Cryptography.KeyedHashAlgorithm",
        "System.Security.Cryptography.MD5",
        "System.Security.Cryptography.Oid",
        "System.Security.Cryptography.PaddingMode",
        "System.Security.Cryptography.RNGCryptoServiceProvider",
        "System.Security.Cryptography.RSA",
        "System.Security.Cryptography.RSAEncryptionPadding",
        "System.Security.Cryptography.RSASignaturePadding",
        "System.Security.Cryptography.SHA1",
        "System.Security.Cryptography.SHA1Managed",
        "System.Security.Cryptography.SHA256",
        "System.Security.Cryptography.SHA256Managed",
        "System.Security.Cryptography.SHA384",
        "System.Security.Cryptography.SHA384Managed",
        "System.Security.Cryptography.SHA512",
        "System.Security.Cryptography.SHA512Managed",
        "System.Security.Cryptography.SymmetricAlgorithm",
        "System.Security.Cryptography.X509Certificates.PublicKey",
        "System.Security.Cryptography.X509Certificates.RSACertificateExtensions",
        "System.Security.Cryptography.X509Certificates.X500DistinguishedName",
        "System.Security.Cryptography.X509Certificates.X509Certificate",
        "System.Security.Cryptography.X509Certificates.X509Certificate2",
        "System.Security.Cryptography.X509Certificates.X509ContentType",
        "System.Security.Cryptography.X509Certificates.X509NameType",
        "System.Single",
        "System.String",
        "System.StringComparer",
        "System.StringComparison",
        "System.StringSplitOptions",
        "System.Text.Encoding",
        "System.Text.RegularExpressions.Capture",
        "System.Text.RegularExpressions.CaptureCollection",
        "System.Text.RegularExpressions.Group",
        "System.Text.RegularExpressions.GroupCollection",
        "System.Text.RegularExpressions.Match",
        "System.Text.RegularExpressions.Regex",
        "System.Text.RegularExpressions.RegexOptions",
        "System.Text.StringBuilder",
        "System.TimeSpan",
        "System.TimeZone",
        "System.TimeZoneInfo.AdjustmentRule",
        "System.TimeZoneInfo.TransitionTime",
        "System.TimeZoneInfo",
        "System.Tuple",
        "System.UInt16",
        "System.UInt32",
        "System.UInt64",
        "System.Uri",
        "System.UriPartial",
        "System.Xml.Linq.Extensions",
        "System.Xml.Linq.XAttribute",
        "System.Xml.Linq.XCData",
        "System.Xml.Linq.XComment",
        "System.Xml.Linq.XContainer",
        "System.Xml.Linq.XDeclaration",
        "System.Xml.Linq.XDocument",
        "System.Xml.Linq.XDocumentType",
        "System.Xml.Linq.XElement",
        "System.Xml.Linq.XName",
        "System.Xml.Linq.XNamespace",
        "System.Xml.Linq.XNode",
        "System.Xml.Linq.XNodeDocumentOrderComparer",
        "System.Xml.Linq.XNodeEqualityComparer",
        "System.Xml.Linq.XObject",
        "System.Xml.Linq.XProcessingInstruction",
        "System.Xml.Linq.XText",
        "System.Xml.XmlNodeType",
        #endregion mslib
        
        #region Newtonsoft.Json
        "Newtonsoft.Json.Formatting",
        "Newtonsoft.Json.JsonConvert",
        "Newtonsoft.Json.Linq.Extensions",
        "Newtonsoft.Json.Linq.JArray",
        "Newtonsoft.Json.Linq.JConstructor",
        "Newtonsoft.Json.Linq.JContainer",
        "Newtonsoft.Json.Linq.JObject",
        "Newtonsoft.Json.Linq.JProperty",
        "Newtonsoft.Json.Linq.JRaw",
        "Newtonsoft.Json.Linq.JToken",
        "Newtonsoft.Json.Linq.JTokenType",
        "Newtonsoft.Json.Linq.JValue",
        #endregion Newtonsoft.Json

        #region Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IApi",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IContext",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IContextApi",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IDeployment",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IGroup",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.ILastError",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IMessageBody",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IOperation",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IPrivateEndpointConnection",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IProduct",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IRequest",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IResponse",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.ISubscription",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.ISubscriptionKeyParameterNames",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IUrl",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IUser",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IUserIdentity",
        #endregion Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context

        #endregion Expression

        #region Mielek.Azure.ApiManagement.PolicyToolkit.Builders
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policy",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.PolicyDocumentBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.PolicyFragmentBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.InboundSectionBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.BackendSectionBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.OutboundSectionBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.OnErrorSectionBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions.ExpressionBuilder<T>",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.AuthenticationBasicPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.AuthenticationCertificatePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.AuthenticationManagedIdentityPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheLookupPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheLookupPolicyBuilder.CacheLookupCachingType",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheLookupPolicyBuilder.CacheLookupDownstreamCachingType",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheLookupValuePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheLookupValuePolicyBuilder.CachingTypeEnum",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheRemoveValuePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheRemoveValuePolicyBuilder.CachingTypeEnum",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheStorePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheStoreValuePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CacheStoreValuePolicyBuilder.CachingTypeEnum",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CheckHeaderPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ChoosePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ChooseWhenBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.CorsPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.EmitMetricPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.EmitMetricDimensionBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.FindAndReplacePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ForwardRequestPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.GetAuthorizationContextPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.GetAuthorizationContextPolicyBuilder.IdentityTypeEnum",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.HttpDataSourcePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.HttpDataSourceHttpRequestBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.HttpDataSourceHttpBackendBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.HttpDataSourceHttpResponseBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.IncludeFragmentPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.IpFilterPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.IpFilterPolicyBuilder.IpFilterAction",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.LimitConcurrencyPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.LogToEventhubPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.MockResponsePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ProxyPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.QuotaByKeyPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.QuotaPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.RateLimitByKeyPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.RateLimitApiOperationBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.RateLimitApiBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.RateLimitPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.RetryPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ReturnResponsePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SendOneWayRequestPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SendRequestPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SendRequestPolicyBuilder.SendRequestMode",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetBodyPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetBodyPolicyBuilder.BodyTemplate",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetBodyPolicyBuilder.XsiNilType",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetHeaderPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetHeaderPolicyBuilder.ExistsActionType",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetStatusPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetVariablePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.TracePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.TracePolicyBuilder.TraceSeverity",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateAzureAdTokenPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateAzureAdTokenClaimBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateAzureAdTokenClaimBuilder.ClaimMatch",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateClientCertificatePolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateClientCertificateIdentityBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateJwtPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateJwtClaimBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.ValidateJwtClaimBuilder.ClaimMatch",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.WaitPolicyBuilder",
        "Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.WaitPolicyBuilder.WaitFor",
        #endregion Mielek.Azure.ApiManagement.PolicyToolkit.Builders
    };

    private readonly static IReadOnlyDictionary<string, IReadOnlySet<string>> AllowedInTypes = new Dictionary<string, IReadOnlySet<String>>()
    {
        { "Newtonsoft.Json.JsonConvert", new HashSet<string>() { "SerializeObject", "DeserializeObject" } },
        { "System.DateTime", new HashSet<string>() { ".ctor", "Add", "AddDays", "AddHours", "AddMilliseconds", "AddMinutes", "AddMonths", "AddSeconds", "AddTicks", "AddYears", "Date", "Day", "DayOfWeek", "DayOfYear", "DaysInMonth", "Hour", "IsDaylightSavingTime", "IsLeapYear", "MaxValue", "Millisecond", "Minute", "MinValue", "Month", "Now", "Parse", "Second", "Subtract", "Ticks", "TimeOfDay", "Today", "ToString", "UtcNow", "Year" } },
        { "System.DateTimeKind", new HashSet<string>() { "Utc" } },
        { "System.Enum", new HashSet<string>() { "Parse", "TryParse", "ToString" } },
        { "System.Net.IPAddress", new HashSet<string>() { "AddressFamily", "Equals", "GetAddressBytes", "IsLoopback", "Parse", "TryParse", "ToString"} },
        { "System.Security.Cryptography.X509Certificates.X500DistinguishedName", new HashSet<string>() { "Name" } },
        { "System.Text.RegularExpressions.Capture", new HashSet<string>() { "Index", "Length", "Value" } },
        { "System.Text.RegularExpressions.CaptureCollection", new HashSet<string>() { "Count", "Item" } },
        { "System.Text.RegularExpressions.Group", new HashSet<string>() { "Captures", "Success" } },
        { "System.Text.RegularExpressions.GroupCollection", new HashSet<string>() { "Count", "Item" } },
        { "System.Text.RegularExpressions.Match", new HashSet<string>() { "Empty", "Groups", "Result" } },
        { "System.Text.RegularExpressions.Regex", new HashSet<string>() { ".ctor", "IsMatch", "Match", "Matches", "Replace", "Unescape", "Split" } },
    };

    private readonly static IReadOnlyDictionary<string, IReadOnlySet<string>> DisallowedInTypes = new Dictionary<string, IReadOnlySet<String>>()
    {
        { "System.Xml.Linq.XDocument", new HashSet<string>() { "Load" } },
    };

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var node = context.Node;
        var libraryAttributeAnnotated = node.Ancestors()
            .OfType<ClassDeclarationSyntax>()
            .Any(c => c.ContainsAttributeOfType(context.SemanticModel, LibraryAttribute));

        if(!libraryAttributeAnnotated) return;

        var nodeSymbol = context.SemanticModel.GetSymbolInfo(node).Symbol;
        if (nodeSymbol == null)
        {
            return;
        }

        var symbol = nodeSymbol.ContainingType;
        var typeName = (symbol.IsGenericType ? symbol.OriginalDefinition : symbol)?.ToFullyQualifiedString() ?? "";
        if (AllowedTypes.Contains(typeName))
        {
            if (AllowedInTypes.TryGetValue(typeName, out var allowed) && !allowed.Contains(nodeSymbol.Name))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.TypeUsed.DisallowedMember, node.GetLocation(), nodeSymbol.Name));
            }
            else if (DisallowedInTypes.TryGetValue(typeName, out var disallowed) && disallowed.Contains(nodeSymbol.Name))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.TypeUsed.DisallowedMember, node.GetLocation(), nodeSymbol.Name));
            }
        }
        else
        {
            context.ReportDiagnostic(Diagnostic.Create(Rules.TypeUsed.DisallowedType, node.GetLocation(), typeName));
        }
    }


}
