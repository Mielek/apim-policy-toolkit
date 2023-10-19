using System.Collections.Immutable;
using System.Xml;

using Mielek.Marshalling.Expressions;
using Mielek.Marshalling.Policies;
using Mielek.Model;
using Mielek.Model.Expressions;

namespace Mielek.Marshalling;
public class Marshaller : IVisitor, IAsyncDisposable, IDisposable
{
    static readonly IReadOnlyDictionary<Type, IMarshallerHandler> Handlers;

    static Marshaller()
    {
        Handlers = new IMarshallerHandler[] {
            #region Roots

            new PolicyDocumentHandler(),
            new PolicyFragmentHandler(),

            #endregion Roots

            #region Policies

            new BasePolicyHandler(),
            new SetBodyPolicyHandler(),
            new SetHeaderPolicyHandler(),
            new SetMethodPolicyHandler(),
            new SetStatusPolicyHandler(),
            new CheckHeaderPolicyHandler(),
            new RateLimitPolicyHandler(),
            new RateLimitByKeyPolicyHandler(),
            new IpFilterPolicyHandler(),
            new GetAuthorizationContextPolicyHandler(),
            new QuotaPolicyHandler(),
            new QuotaByKeyPolicyHandler(),
            new ValidateAzureAdTokenPolicyHandler(),
            new ValidateJwtPolicyHandler(),
            new ValidateClientCertificatePolicyHandler(),
            new ChoosePolicyHandler(),
            new ForwardRequestPolicyHandler(),
            new IncludeFragmentPolicyHandler(),
            new LimitConcurrencyPolicyHandler(),
            new LogToEventhubPolicyHandler(),
            new EmitMetricPolicyHandler(),
            new MockResponsePolicyHandler(),
            new RetryPolicyHandler(),
            new ReturnResponsePolicyHandler(),
            new SendOneWayRequestPolicyHandler(),
            new SendRequestPolicyHandler(),
            new ProxyPolicyHandler(),
            new SetVariablePolicyHandler(),
            new TracePolicyHandler(),
            new WaitPolicyHandler(),
            new AuthenticationBasicPolicyHandler(),
            new AuthenticationCertificatePolicyHandler(),
            new AuthenticationManagedIdentityPolicyHandler(),
            new CacheLookupPolicyHandler(),
            new CacheStorePolicyHandler(),

            #endregion Policies

            #region Expressions

            new ConstantExpressionHandler<string>(),
            new ConstantExpressionHandler<bool>(),
            new ConstantExpressionHandler<int>(),
            new ConstantExpressionHandler<uint>(),
            new ConstantExpressionHandler<object>(),

            new InlineExpressionHandler<string>(),
            new InlineExpressionHandler<bool>(),
            new InlineExpressionHandler<int>(),
            new InlineExpressionHandler<uint>(),
            new InlineExpressionHandler<object>(),

            new LambdaExpressionHandler<string>(),
            new LambdaExpressionHandler<bool>(),
            new LambdaExpressionHandler<int>(),
            new LambdaExpressionHandler<uint>(),
            new LambdaExpressionHandler<object>(),

            new MethodExpressionHandler<string>(),
            new MethodExpressionHandler<bool>(),
            new MethodExpressionHandler<int>(),
            new MethodExpressionHandler<uint>(),
            new MethodExpressionHandler<object>(),

            #endregion Expressions
        }.ToImmutableDictionary(_ => _.Type);
    }


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
        internal void WriteElement<T>(string name, T value)
        {
            WriteElement(name, $"{value}");
        }
        internal void WriteElement<T>(string name, IExpression<T> expression)
        {
            BaseWriter.WriteStartElement(name);
            expression.Accept(_marshaller);
            BaseWriter.WriteEndElement();
        }
        internal void WriteNullableElement<T>(string name, IExpression<T>? expression)
        {
            if (expression != null)
            {
                WriteElement(name, expression);
            }
        }

        internal void WriteStartElement(string name) => BaseWriter.WriteStartElement(name);
        internal void WriteEndElement() => BaseWriter.WriteEndElement();

        internal void WriteAttribute(string name, string value) => BaseWriter.WriteAttributeString(name, value);
        internal void WriteAttribute<T>(string name, T value) => BaseWriter.WriteAttributeString(name, $"{value}");
        internal void WriteAttribute<T>(string name, IExpression<T> expression)
        {
            BaseWriter.WriteStartAttribute(name);
            expression.Accept(_marshaller);
            BaseWriter.WriteEndAttribute();
        }

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
        internal void WriteNullableAttribute<T>(string name, IExpression<T>? expression)
        {
            if (expression != null)
            {
                WriteAttribute(name, expression);
            }
        }



        internal void WriteExpression<T>(IExpression<T> expression) => expression.Accept(_marshaller);

        internal void WriteRawString(string value) => BaseWriter.WriteRaw(value);

        internal void WriteElementCollection(string collectionElement, ICollection<string> values)
        {
            foreach (var value in values)
            {
                BaseWriter.WriteElementString(collectionElement, value);
            }
        }

        internal void WriteElementCollection(string collectionName, string collectionElement, ICollection<string> values)
        {
            BaseWriter.WriteStartElement(collectionName);
            WriteElementCollection(collectionElement, values);
            BaseWriter.WriteEndElement();
        }

        internal void WriteNullableElementCollection(string collectionElement, ICollection<string>? values)
        {
            if (values == null || values.Count == 0) return;

            WriteElementCollection(collectionElement, values);
        }

        internal void WriteNullableElementCollection(string collectionName, string collectionElement, ICollection<string>? values)
        {
            if (values == null || values.Count == 0) return;

            BaseWriter.WriteStartElement(collectionName);
            WriteElementCollection(collectionElement, values);
            BaseWriter.WriteEndElement();
        }

        internal void WriteNullableElementCollection<T>(string collectionName, string collectionElement, ICollection<IExpression<T>>? values)
        {
            if (values == null || values.Count == 0) return;

            BaseWriter.WriteStartElement(collectionName);
            foreach (var value in values)
            {
                WriteElement(collectionElement, value);
            }
            BaseWriter.WriteEndElement();
        }
    }
}