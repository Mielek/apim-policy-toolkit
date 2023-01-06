using System.Xml;

using Mielek.Marshalling.Expressions;
using Mielek.Marshalling.Policies;
using Mielek.Model;
using Mielek.Model.Expressions;
using Mielek.Model.Policies;

namespace Mielek.Marshalling;
public class Marshaller : IVisitor, IAsyncDisposable, IDisposable
{
    static readonly Dictionary<Type, IMarshallerHandler> Handlers = new Dictionary<Type, IMarshallerHandler>()
    {
        #region Roots
        { typeof(PolicyDocument), new PolicyDocumentHandler() },
        { typeof(PolicyFragment), new PolicyFragmentHandler() },
        #endregion Roots

        #region Policies
        { typeof(BasePolicy), new BasePolicyHandler() },
        { typeof(SetBodyPolicy), new SetBodyPolicyHandler() },
        { typeof(SetHeaderPolicy), new SetHeaderPolicyHandler() },
        { typeof(SetMethodPolicy), new SetMethodPolicyHandler() },
        { typeof(SetStatusPolicy), new SetStatusPolicyHandler() },
        { typeof(CheckHeaderPolicy), new CheckHeaderPolicyHandler() },
        { typeof(RateLimitPolicy), new RateLimitPolicyHandler() },
        { typeof(RateLimitByKeyPolicy), new RateLimitByKeyPolicyHandler() },
        { typeof(IpFilterPolicy), new IpFilterPolicyHandler() },
        { typeof(GetAuthorizationContextPolicy), new GetAuthorizationContextPolicyHandler() },
        { typeof(QuotaPolicy), new QuotaPolicyHandler() },
        { typeof(QuotaByKeyPolicy), new QuotaByKeyPolicyHandler() },
        { typeof(ValidateAzureAdTokenPolicy), new ValidateAzureAdTokenPolicyHandler() },
        { typeof(ValidateJwtPolicy), new ValidateJwtPolicyHandler() },
        { typeof(ValidateClientCertificatePolicy), new ValidateClientCertificatePolicyHandler() },
        { typeof(ChoosePolicy), new ChoosePolicyHandler() },
        { typeof(ForwardRequestPolicy), new ForwardRequestPolicyHandler() },
        { typeof(IncludeFragmentPolicy), new IncludeFragmentPolicyHandler() },
        { typeof(LimitConcurrencyPolicy), new LimitConcurrencyPolicyHandler() },
        { typeof(LogToEventhubPolicy), new LogToEventhubPolicyHandler() },
        { typeof(EmitMetricPolicy), new EmitMetricPolicyHandler() },
        #endregion Policies

        #region Expressions
        { typeof(ConstantExpression<string>), new ConstantExpressionHandler<string>() },
        { typeof(ConstantExpression<bool>), new ConstantExpressionHandler<bool>() },
        { typeof(InlineScriptExpression<string>), new InlineScriptExpressionHandler<string>() },
        { typeof(InlineScriptExpression<bool>), new InlineScriptExpressionHandler<bool>() },
        { typeof(FileScriptExpression<string>), new FileScriptExpressionHandler<string>() },
        { typeof(FileScriptExpression<bool>), new FileScriptExpressionHandler<bool>() },
        { typeof(FunctionFileScriptExpression<string>), new FunctionFileScriptExpressionHandler<string>() },
        { typeof(FunctionFileScriptExpression<bool>), new FunctionFileScriptExpressionHandler<bool>() },

        #endregion Expressions
    };

    #region Create
    public static Marshaller Create(XmlWriter xmlWriter) => Create(xmlWriter, MarshallerOptions.Default);
    public static Marshaller Create(XmlWriter xmlWriter, MarshallerOptions options) => new Marshaller(xmlWriter, options);
    public static Marshaller Create(Stream output) => Create(output, MarshallerOptions.Default);
    public static Marshaller Create(Stream output, MarshallerOptions options) => new Marshaller(XmlWriter.Create(output, options.XmlWriterSettings), options);
    public static Marshaller Create(TextWriter output) => Create(output, MarshallerOptions.Default);
    public static Marshaller Create(TextWriter output, MarshallerOptions options) => new Marshaller(XmlWriter.Create(output, options.XmlWriterSettings), options);
    public static Marshaller Create(string outputFileName) => Create(outputFileName, MarshallerOptions.Default);
    public static Marshaller Create(string outputFileName, MarshallerOptions options) => new Marshaller(XmlWriter.Create(outputFileName, options.XmlWriterSettings), options);
    #endregion Create

    internal InternalWriter Writer { get; init; }

    Marshaller(XmlWriter xmlWriter, MarshallerOptions options)
    {
        Writer = new InternalWriter(xmlWriter, this);
        Options = options;
    }

    public MarshallerOptions Options { get; }

    public void Flush() => Writer.BaseWriter.Flush();

    public void Dispose() => Writer.BaseWriter.Dispose();

    public ValueTask DisposeAsync() => Writer.BaseWriter.DisposeAsync();

    public void Visit<T>(T element) where T : IVisitable
    {
        if (!Handlers.TryGetValue(typeof(T), out var handler))
        {
            throw new Exception();
        }

        handler.Marshal(this, element);
    }

    internal class InternalWriter
    {
        readonly Marshaller _marshaller;
        internal XmlWriter BaseWriter;

        internal InternalWriter(XmlWriter writer, Marshaller marshaller)
        {
            BaseWriter = writer;
            _marshaller = marshaller;
        }

        internal void WriteElement(string name, string? value = null) => BaseWriter.WriteElementString(name, value);

        internal void WriteStartElement(string name) => BaseWriter.WriteStartElement(name);
        internal void WriteEndElement() => BaseWriter.WriteEndElement();

        internal void WriteAttribute(string name, string value) => BaseWriter.WriteAttributeString(name, value);
        internal void WriteAttribute<T>(string name, T value) => BaseWriter.WriteAttributeString(name, $"{value}");

        internal void WriteNullableAttribute(string name, string? value)
        {
            if (value != null)
            {
                BaseWriter.WriteAttributeString(name, value);
            }
        }
        internal void WriteNullableAttribute<T>(string name, T? value)
        {
            if (value != null)
            {
                BaseWriter.WriteAttributeString(name, $"{value}");
            }
        }

        internal void WriteExpressionAsAttribute<T>(string name, IExpression<T> expression)
        {
            BaseWriter.WriteStartAttribute(name);
            expression.Accept(_marshaller);
            BaseWriter.WriteEndAttribute();
        }

        internal void WriteNullableExpressionAsAttribute<T>(string name, IExpression<T>? expression)
        {
            if (expression != null)
            {
                WriteExpressionAsAttribute(name, expression);
            }
        }

        internal void WriteExpressionAsElement<T>(string name, IExpression<T> expression)
        {
            BaseWriter.WriteStartElement(name);
            expression.Accept(_marshaller);
            BaseWriter.WriteEndElement();
        }

        internal void WriteExpression<T>(IExpression<T> expression) => expression.Accept(_marshaller);

        internal void WriteRawString(string value) => BaseWriter.WriteRaw(value);

        internal void WriteElementCollection(string collectionName, string collectionElement, ICollection<string> values)
        {
            BaseWriter.WriteStartElement(collectionName);
            foreach (var value in values)
            {
                BaseWriter.WriteElementString(collectionElement, value);
            }
            BaseWriter.WriteEndElement();
        }

        internal void WriteNullableElementCollection(string collectionName, string collectionElement, ICollection<string>? values)
        {
            if (values == null || values.Count == 0) return;

            BaseWriter.WriteStartElement(collectionName);
            foreach (var value in values)
            {
                BaseWriter.WriteElementString(collectionElement, value);
            }
            BaseWriter.WriteEndElement();
        }

        internal void WriteNullableElementCollection<T>(string collectionName, string collectionElement, ICollection<IExpression<T>>? values)
        {
            if (values == null || values.Count == 0) return;

            BaseWriter.WriteStartElement(collectionName);
            foreach (var value in values)
            {
                WriteExpressionAsElement(collectionElement, value);
            }
            BaseWriter.WriteEndElement();
        }
    }
}